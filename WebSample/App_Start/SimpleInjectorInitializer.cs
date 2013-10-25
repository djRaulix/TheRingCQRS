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

    using MassTransit;
    using MassTransit.BusConfigurators;
    using MassTransit.Saga;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web.Mvc;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.RavenDb;
    using TheRing.CQRS.MassTransit;
    using TheRing.MassTransit.RavenDb;
    using TheRing.RavenDb;

    using WebSample.Domain;
    using WebSample.Domain.UserAgg;
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
            var readModelHandlers = LoadEventLayer(container).ToList();
            var sagaHandlers = LoadSagaLayer().ToList();

            LoadMassTransitImplementation(container, commandHandlers, readModelHandlers);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            LoadSagas(container, sagaHandlers);
        }

        private static IEnumerable<KeyValuePair<Type, Func<object>>> LoadCommandLayer(Container container)
        {
            container.RegisterSingleOpenGeneric(typeof(IAggregateRootRepository<>), typeof(AggregateRootRepository<>));
            container.RegisterOpenGeneric(typeof(CommandConsumer<>), typeof(CommandConsumer<>));

            var commandMapper = new CommandMapper();
            commandMapper.AddMappings(typeof(User).Assembly);

            foreach (var mapping in commandMapper.Mappings)
            {
                container.RegisterSingle(mapping.HandlesCommandType, mapping.CommandHandlerType);
                var commandConsumerType = typeof(CommandConsumer<>).MakeGenericType(mapping.CommandType);
                yield return
                    new KeyValuePair<Type, Func<object>>(
                        commandConsumerType,
                        () => container.GetInstance(commandConsumerType));
            }
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
                    var factory = container.GetInstance<ICqrsDocumentStoreFactory>();
                    if (context.ImplementationType == typeof(EventStore))
                    {
                        return factory.EventStore;
                    }

                    if (context.ImplementationType != null && context.ImplementationType.IsGenericType &&
                        context.ImplementationType.GetGenericTypeDefinition() == typeof(SagaRepository<>))
                    {
                        return factory.SagaStore;
                    }

                    return factory.ReadModel;
                });

            container.RegisterSingle<IUserRepository, UserRepository>();
            container.RegisterSingle<IDenormalizerRepository, DenormalizerRepository>();
            container.RegisterSingle<IEventStore, EventStore>();
            container.RegisterSingleOpenGeneric(typeof(ISagaRepository<>), typeof(SagaRepository<>));
        }

        private static IEnumerable<KeyValuePair<Type, Func<object>>> LoadEventLayer(Container container)
        {
            container.RegisterSingle<NoSnapshotKeeper>();
            container.RegisterSingle<InMemorySnapshotKeeper>();
            
            container.RegisterWithContext<ISnapshotKeeper>(
                context =>
                {
                    return container.GetInstance<InMemorySnapshotKeeper>();
                });

            container.RegisterSingle<IEventSourcedFactory>(() => new EventSourcedFactory(container.GetInstance));
            container.RegisterSingle<IEventSourcedRepository, EventSourcedRepository>();
            var mapper = new DenormalizerMapper();
            mapper.AddMappings(typeof(UserViewDenormalizer).Assembly);

            foreach (var denormalizer in mapper.Mappings)
            {
                var denormalizerType = denormalizer.DenormalizerType;
                container.RegisterSingle(denormalizerType, denormalizerType);
                foreach (var eventType in denormalizer.EventTypes)
                {
                    var wrapper = typeof(DenormalizerConsumer<>).MakeGenericType(eventType);
                    yield return
                        new KeyValuePair<Type, Func<object>>(
                            wrapper,
                            () => Activator.CreateInstance(wrapper, container.GetInstance(denormalizerType)));
                }
            }
        }

        private static IEnumerable<Type> LoadSagaLayer()
        {
            return from type in typeof(CreateUserSaga).Assembly.GetExportedTypes()
                where
                    !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType &&
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