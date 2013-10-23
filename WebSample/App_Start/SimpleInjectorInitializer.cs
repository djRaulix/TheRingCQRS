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
    using TheRing.CQRS.Commanding.MassTransit;
    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.MassTransit;
    using TheRing.CQRS.Eventing.RavenDb;
    using TheRing.CQRS.EventSourcedDomain;
    using TheRing.CQRS.MassTransit;
    using TheRing.MassTransit.RavenDb;
    using TheRing.RavenDb;

    using WebSample.Commanding;
    using WebSample.Domain;
    using WebSample.ReadModel;
    using WebSample.Sagas;

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

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        #endregion

        #region Methods

        private static void InitializeContainer(Container container)
        {
            LoadRavenDbImplementation(container);

            var commandHandlers = LoadCommandLayer(container).ToList();
            var readModelHandlers = LoadReadModelLayer(container).ToList();
            var sagaHandlers = LoadSagaLayer().ToList();

            LoadMassTransitImplementation(container, commandHandlers, readModelHandlers);

            LoadEventSourcedDomainLayer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            LoadSagas(container, sagaHandlers);
        }

        private static IEnumerable<KeyValuePair<Type, Func<object>>> LoadCommandLayer(Container container)
        {
            container.RegisterSingleOpenGeneric(typeof(IEditAggregate<>), typeof(EditAggregate<>));
            container.RegisterSingleOpenGeneric(typeof(CommandHandler<,>), typeof(CommandHandler<,>));
            container.RegisterSingleOpenGeneric(typeof(CommandConsumer<>), typeof(CommandConsumer<>));

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
                    var commandType = args.Last();
                    var commandHandlerType = typeof(CommandHandler<,>).MakeGenericType(args);
                    var handlesCommandType = typeof(IHandlesCommand<>).MakeGenericType(commandType);
                    container.RegisterSingle(handlesCommandType, () => container.GetInstance(commandHandlerType));
                    var commandConsumerType = typeof(CommandConsumer<>).MakeGenericType(commandType);
                    yield return
                        new KeyValuePair<Type, Func<object>>(
                            commandConsumerType,
                            () => container.GetInstance(commandConsumerType));
                }
            }
        }

        private static void LoadEventSourcedDomainLayer(Container container)
        {
            var aggregates =
                typeof(User).Assembly.GetExportedTypes().Where(t => t.Implements<EventSourcedAggregateRoot>()).ToList();

            foreach (var aggregate in aggregates)
            {
                container.Register(aggregate, aggregate, Lifestyle.Transient);
            }

            container.RegisterSingle<IEventSourcedFactory>(() => new EventSourcedFactory(container.GetInstance));
            container.RegisterSingleOpenGeneric(typeof(IEventSourcedRepository<>),typeof(EventSourcedRepository<>));
            container.RegisterSingleOpenGeneric(typeof(IAggregateRootRepository<>), typeof(EventSourcedAggregateRootRepository<>));
        }

        private static void LoadMassTransitImplementation(
            Container container,
            IEnumerable<KeyValuePair<Type, Func<object>>> commandConsumers,
            IEnumerable<KeyValuePair<Type, Func<object>>> publishConsumers)
        {
            var factory = new BusFactory(container.GetInstance);
            container.RegisterSingle<IBusFactory>(() => factory);

            Action<ServiceBusConfigurator> config = sbc => sbc.UseRabbitMq();

            factory.Set(Constants.RequestQueue, commandConsumers, moreConfig: config);

            factory.Set(Constants.ResponseQueue, moreConfig: config);

            factory.Set(Constants.ReadModelQueue, publishConsumers, moreConfig: config);

            container.RegisterSingle(factory.Get(Constants.ReadModelQueue).EventBus());
            container.RegisterSingle(
                factory.Get(Constants.ResponseQueue).CommandBus(Constants.RequestQueue));
        }

        private static void LoadRavenDbImplementation(Container container)
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

                    if (context.ImplementationType != null && context.ImplementationType.IsGenericType &&
                        context.ImplementationType.GetGenericTypeDefinition() == typeof(SagaRepository<>))
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

        private static IEnumerable<KeyValuePair<Type, Func<object>>> LoadReadModelLayer(Container container)
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

        private static IEnumerable<Type> LoadSagaLayer()
        {
            return from type in typeof(CreateUserSaga).Assembly.GetExportedTypes()
                where
                    !type.IsAbstract && type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(WebSampleSaga<>)
                select type;
        }

        private static void LoadSagas(Container container, IEnumerable<Type> sagaConsumers)
        {
            container.GetInstance<IBusFactory>()
                .Set(Constants.SagaQueue, sagas: sagaConsumers, moreConfig: sbc => sbc.UseRabbitMq());
        }

        #endregion
    }
}