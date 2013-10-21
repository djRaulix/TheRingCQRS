#region using

using WebActivator;

using WebSample.App_Start;

#endregion

[assembly: PostApplicationStartMethod(typeof(SimpleInjectorInitializer), "Initialize")]

namespace WebSample.App_Start
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    using Magnum.Extensions;
    using Magnum.Reflection;

    using MassTransit;
    using MassTransit.BusConfigurators;
    using MassTransit.Saga;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web.Mvc;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.MassTransit;
    using TheRing.CQRS.MassTransit.Commanding;
    using TheRing.CQRS.MassTransit.Querying;
    using TheRing.CQRS.RavenDb;
    using TheRing.MassTransit.RavenDb;
    using TheRing.RavenDb;

    using WebSample.Commanding;
    using WebSample.Domain;
    using WebSample.ReadModel;

    #endregion

    public static class SimpleInjectorInitializer
    {
        #region Public Methods and Operators

        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? Go to: http://bit.ly/YE8OJj.
            var container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        public static void LoadMassTransitImplementation(
            Container container, 
            IEnumerable<KeyValuePair<Type, Func<object>>> commandConsumers, 
            IEnumerable<KeyValuePair<Type, Func<object>>> publishConsumers, 
            IEnumerable<Type> sagaConsumers)
        {
            var factory = new BusFactory(container.GetInstance);

            Action<ServiceBusConfigurator> config = sbc => sbc.UseRabbitMq();

            factory.Set(Constants.RequestQueue, commandConsumers, moreConfig: config);

            factory.Set(Constants.ResponseQueue, moreConfig: config);

            factory.Set(Constants.ReadModelQueue, publishConsumers, moreConfig: config);

            factory.Set(Constants.SagaQueue, sagas: sagaConsumers, moreConfig: config);

            container.RegisterSingle(factory.Get(Constants.ReadModelQueue).EventBus());
            container.RegisterSingle(
                factory.Get(Constants.ResponseQueue).CommandBus(Constants.RequestQueue));
        }

        public static void LoadRavenDbImplementation(Container container)
        {
            container.RegisterSingle<ICreateStoreStrategy, CreateStoreStrategy>();
            var storeFactoryRegistration =
                Lifestyle.Singleton.CreateRegistration<DocumentStoreFactory, DocumentStoreFactory>(container);
            container.AddRegistration(typeof(IDocumentStoreFactory), storeFactoryRegistration);
            container.AddRegistration(typeof(IAddDocumenStoreFromParameters), storeFactoryRegistration);
            container.AddRegistration(typeof(IAddDocumenStore), storeFactoryRegistration);

            container.Register<ICqrsDocumentStoreFactoryInitializer, CqrsDocumentStoreFactoryInitializer>();
            container.RegisterSingle<ICqrsDocumentStoreFactory, CqrsDocumentStoreFactory>();
            container.RegisterWithContext(
                context =>
                {
                    if (context.ImplementationType == typeof(EventStore))
                    {
                        return container.GetInstance<ICqrsDocumentStoreFactory>().EventStore;
                    }

                    if (context.ImplementationType == typeof(SagaRepository<>))
                    {
                        return container.GetInstance<ICqrsDocumentStoreFactory>().SagaStore;
                    }

                    return container.GetInstance<ICqrsDocumentStoreFactory>().ReadModel;
                });

            container.RegisterSingle<IUserRepository, UserRepository>();
            container.RegisterSingle<IDenormalizerRepository, DenormalizerRepository>();
            container.RegisterSingle<IEventStore, EventStore>();
            container.RegisterSingleOpenGeneric(typeof(ISagaRepository<>), typeof(SagaRepository<>));
        }

        public static IEnumerable<KeyValuePair<Type, Func<object>>> LoadReadModelLayer(Container container)
        {
            var denormalizerType = typeof(IDenormalizeEvent<>);

            var denormalizers = from type in typeof(UserViewDenormalizer).Assembly.GetExportedTypes()
                from @interface in
                    type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == denormalizerType)
                group @interface by type
                into grp
                where grp.Any()
                select new { type = grp.Key, interfaces = grp.ToList() };

            foreach (var denormalizer in denormalizers)
            {
                var denType = denormalizer.type;

                container.RegisterSingle(
                    denType, 
                    () => FastActivator.Create(denType, container.GetInstance<IDenormalizerRepository>()));

                foreach (var @interface in denormalizer.interfaces)
                {
                    var eventType = @interface.GenericTypeArguments.First();
                    var wrapper = typeof(Denormalizer<,>).MakeGenericType(denType, eventType);
                    yield return
                        new KeyValuePair<Type, Func<object>>(
                            wrapper, 
                            () => Activator.CreateInstance(wrapper, container.GetInstance(denType)));
                }
            }
        }

        public static IEnumerable<Type> LoadSagaLayer()
        {
            /*return from type in typeof(CreateUserSaga).Assembly.GetExportedTypes()
                   where type.IsClass && !type.IsAbstract && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(SagaStateMachineBase<>))
                   select type;   */
            return Enumerable.Empty<Type>();
        }

        #endregion

        #region Methods

        private static void InitializeContainer(Container container)
        {
            LoadRavenDbImplementation(container);

            var commandHandlers = LoadCommandLayer(container).ToList();
            var readModelHandlers = LoadReadModelLayer(container).ToList();
            var sagaHandlers = LoadSagaLayer().ToList();

            LoadMassTransitImplementation(container, commandHandlers, readModelHandlers, sagaHandlers);

            LoadDomainLayer(container);
        }

        private static IEnumerable<KeyValuePair<Type, Func<object>>> LoadCommandLayer(Container container)
        {
            container.RegisterSingleOpenGeneric(typeof(IEditAggregate<>), typeof(EditAggregate<>));
            container.RegisterSingleOpenGeneric(typeof(CommandHandler<,>), typeof(CommandHandler<,>));

            var runCommandType = typeof(IRunCommand<,>);
            var commandRunners = from type in typeof(CreateUserCommand).Assembly.GetExportedTypes()
                from @interface in
                    type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == runCommandType)
                group @interface by type
                into grp
                where grp.Any()
                select new { type = grp.Key, interfaces = grp.ToList() };

            foreach (var runner in commandRunners)
            {
                var registration = Lifestyle.Singleton.CreateRegistration(runner.type, runner.type, container);

                foreach (var item in runner.interfaces)
                {
                    container.AddRegistration(item, registration);
                    var args = item.GetGenericArguments();
                    var commandHandlerType = typeof(CommandHandler<,>).MakeGenericType(args);
                    yield return
                        new KeyValuePair<Type, Func<object>>(
                            commandHandlerType, 
                            () => container.GetInstance(commandHandlerType));
                }
            }
        }

        private static void LoadDomainLayer(Container container)
        {
            var aggregates =
                typeof(User).Assembly.GetExportedTypes().Where(t => t.Implements<AggregateRoot>()).ToList();

            foreach (var aggregate in aggregates)
            {
                container.Register(aggregate, aggregate, Lifestyle.Transient);
            }

            container.RegisterSingle<IAggregateRootFactory>(() => new AggregateRootFactory(container));

            container.RegisterSingle<IRepository, Repository>();
        }

        #endregion
    }
}