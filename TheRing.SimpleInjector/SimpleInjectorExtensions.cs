namespace TheRing.SimpleInjector
{
    #region using

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using global::SimpleInjector;
    using global::SimpleInjector.Advanced;

    #endregion

    public static class SimpleInjectorExtensions
    {
        #region Public Methods and Operators

        public static void AllowResolvingFuncFactories(
            this ContainerOptions options)
        {
            options.Container.ResolveUnregisteredType += (s, e) =>
            {
                var type = e.UnregisteredServiceType;

                if (!type.IsGenericType ||
                    type.GetGenericTypeDefinition() != typeof(Func<>))
                {
                    return;
                }

                Type serviceType = type.GetGenericArguments().First();

                InstanceProducer registration = options.Container
                    .GetRegistration(serviceType, true);

                Type funcType =
                    typeof(Func<>).MakeGenericType(serviceType);

                var factoryDelegate = Expression.Lambda(
                    funcType,
                    registration.BuildExpression()).Compile();

                e.Register(Expression.Constant(factoryDelegate));
            };
        }

        public static void AutowirePropertiesWithAttribute<TAttribute>(
            this ContainerOptions options)
            where TAttribute : Attribute
        {
            options.PropertySelectionBehavior =
                new AttributePropertyInjectionBehavior(
                    options.PropertySelectionBehavior,
                    typeof(TAttribute));
        }

        public static void RegisterFuncFactory<TService, TImpl>(
            this Container container,
            Lifestyle lifestyle = null)
            where TService : class
            where TImpl : class, TService
        {
            lifestyle = lifestyle ?? Lifestyle.Transient;

            // Register the Func<T> that resolves that instance.
            container.RegisterSingle(
                () =>
                {
                    var producer = new InstanceProducer(
                        typeof(TService),
                        lifestyle.CreateRegistration<TService, TImpl>(container));

                    Func<TService> instanceCreator =
                        () => (TService)producer.GetInstance();

                    if (container.IsVerifying())
                    {
                        instanceCreator.Invoke();
                    }

                    return instanceCreator;
                });
        }

        #endregion

        private sealed class AttributePropertyInjectionBehavior
            : IPropertySelectionBehavior
        {
            #region Fields

            private readonly Type attribute;
            private readonly IPropertySelectionBehavior behavior;

            #endregion

            #region Constructors and Destructors

            public AttributePropertyInjectionBehavior(
                IPropertySelectionBehavior baseBehavior,
                Type attributeType)
            {
                this.behavior = baseBehavior;
                this.attribute = attributeType;
            }

            #endregion

            #region Public Methods and Operators

            public bool SelectProperty(Type type, PropertyInfo prop)
            {
                return this.IsPropertyDecorated(prop) ||
                       this.behavior.SelectProperty(type, prop);
            }

            #endregion

            #region Methods

            private bool IsPropertyDecorated(PropertyInfo p)
            {
                return p.GetCustomAttributes(this.attribute, true).Any();
            }

            #endregion
        }
    }
}