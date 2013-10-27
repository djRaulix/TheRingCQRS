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

    using TheRing.CQRS.Application;
    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.Commanding.Handler;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.Bus;
    using TheRing.CQRS.Eventing.EventSourced.Factory;
    using TheRing.CQRS.Eventing.EventSourced.Repository;
    using TheRing.CQRS.Eventing.EventSourced.Snapshot;
    using TheRing.CQRS.Eventing.EventStore;
    using TheRing.CQRS.Eventing.RavenDb;
    using TheRing.CQRS.MassTransit;
    using TheRing.CQRS.MassTransit.RavenDb;
    using TheRing.RavenDb;

    using WebSample.Domain.User;
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
            container.RegisterOpenGeneric(typeof(HandleContext<>), typeof(HandleContext<>));
            container.RegisterSingle<IHandleException,ExceptionHandler>();
            container.RegisterSingleDecorator(
                typeof(IHandleException),
                typeof(NewerEventSourcedConcurrencyHandleExceptionDecorator));
            container.RegisterSingleDecorator(typeof(IHandleException), typeof(ConcurrencyHandleExceptionDecorator));
            container.RegisterSingleOpenGeneric(typeof(IHandleCommand<>), typeof(CommandHandler<>));

            var runners =
                from r in typeof(User).Assembly.GetExportedTypes().Where(t => !t.IsAbstract)
                from i in
                    r.GetInterfaces()
                        .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRunCommand<,>))
                group i by r
                into grp
                where grp.Any()
                select grp;

            foreach (var runner in runners)
            {
                var registration = Lifestyle.Singleton.CreateRegistration(runner.Key, runner.Key, container);
                foreach (var type in runner)
                {
                    container.AddRegistration(type, registration);
                    var args = type.GenericTypeArguments;
                    var command = args[1];
                    var commandRunnerType = typeof(CommandRunner<,>).MakeGenericType(args);
                    var runCommandInterface = typeof(IRunCommand<>).MakeGenericType(command);

                    container.RegisterSingle(runCommandInterface, commandRunnerType);
                    var contextHandler = typeof(HandleContext<>).MakeGenericType(command);

                    yield return
                        new KeyValuePair<Type, Func<object>>(
                            contextHandler,
                            () => container.GetInstance(contextHandler));
                }
            }
        }

        private static IEnumerable<KeyValuePair<Type, Func<object>>> LoadEventLayer(Container container)
        {
            container.RegisterSingleOpenGeneric(typeof(EventConsumer<>), typeof(EventConsumer<>));
            container.RegisterSingle<IEventSourcedFactory>(() => new EventSourcedFactory(container.GetInstance));
            container.RegisterSingleOpenGeneric(typeof(IEventSourcedRepository<>), typeof(EventSourcedRepository<>));
            container.RegisterSingleDecorator(
                typeof(IEventSourcedRepository<User>),
                typeof(SnaphotEventSourcedRepositoryDecorator<User>));
            container.RegisterSingle<InMemorySnapshotKeeper>();

            container.RegisterWithContext<ISnapshotKeeper>(
                context => { return container.GetInstance<InMemorySnapshotKeeper>(); });

            var subscribers = from type in typeof(User).Assembly.GetExportedTypes()
                where !type.IsAbstract
                from @interface in
                    type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ISubscribeEvent<>)
                group @interface by type
                into grp
                where grp.Any()
                select grp;

            foreach (var subscriber in subscribers)
            {
                var registration = Lifestyle.Singleton.CreateRegistration(subscriber.Key, subscriber.Key, container);
                foreach (var type in subscriber)
                {
                    container.AddRegistration(type, registration);

                    var eventConsumer = typeof(EventConsumer<>).MakeGenericType(type.GenericTypeArguments.Single());
                    yield return
                        new KeyValuePair<Type, Func<object>>(
                            eventConsumer,
                            () => container.GetInstance(eventConsumer));
                }
            }
        }

        private static void LoadMassTransitImplementation(
            Container container,
            IEnumerable<KeyValuePair<Type, Func<object>>> commandConsumers,
            IEnumerable<KeyValuePair<Type, Func<object>>> eventConsumers)
        {
            var factory = new BusFactory(container.GetInstance);
            container.RegisterSingle<IBusFactory>(() => factory);

            Action<ServiceBusConfigurator> config = sbc => sbc.UseRabbitMq();
            factory.InitCommanding(commandConsumers, config);

            factory.InitEventing(eventConsumers, config);
            container.RegisterSingle<IEventBus, EventBus>();
            container.RegisterSingle<ICommandBus, CommandBus>();
            container.RegisterWithContext(
                context =>
                {
                    if (context.ImplementationType == typeof(CommandBus))
                    {
                        return factory.RequestQueue();
                    }
                    return factory.ReadModelQueue();
                });
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

        private static IEnumerable<Type> LoadSagaLayer()
        {
            return from type in typeof(CreateUserSaga).Assembly.GetExportedTypes()
                where
                    !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(AbstractSagaStateMachineBase<>)
                select type;
        }

        private static void LoadSagas(Container container, IEnumerable<Type> sagaConsumers)
        {
            container.GetInstance<IBusFactory>().InitSagas(sagaConsumers, sbc => sbc.UseRabbitMq());
        }

        #endregion
    }
}