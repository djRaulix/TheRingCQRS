namespace TheRing.CQRS.RavenDb.Test
{
    #region using

    using System;
    using System.Collections.Generic;

    using FakeItEasy;

    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.MassTransit;
    using TheRing.CQRS.MassTransit.Test.Fakes;

    #endregion

    public static class GlobalSpecification
    {
        #region Constants

        public const string CommandRequestQueue = "loopback://localhost/request_queue";
        public const string CommandResponseQueue = "loopback://localhost/response_queue";
        public const string EventQueue = "loopback://localhost/event_queue";

        #endregion

        #region Static Fields

        private static readonly object LockObject = new object();
        private static IBusFactory busFactory;

        #endregion

        #region Public Properties

        public static IBusFactory BusFactory
        {
            get
            {
                if (busFactory == null)
                {
                    lock (LockObject)
                    {
                        if (busFactory == null)
                        {
                            busFactory = new BusFactory(
                                type =>
                                {
                                    var method = typeof(A).GetMethod("Fake");
                                    var generic = method.MakeGenericMethod(type);
                                    return generic.Invoke(null, null);
                                });

                            FakeEventDenormalizer = new FakeEventDenormalizer();
                            busFactory.Set(
                                EventQueue,
                                new[]
                                {
                                    new KeyValuePair<Type, Func<object>>(
                                        typeof(EventConsumer<>),
                                        () =>
                                            new EventConsumer<FakeEvent>(FakeEventDenormalizer))
                                });

                            FakeCommandHandler = new FakeCommandHandler();

                            busFactory.Set(
                                CommandRequestQueue,
                                new[]
                                {
                                    new KeyValuePair<Type, Func<object>>(
                                        typeof(MassTransitHandleContext<FakeCommand>),
                                        () =>
                                            new MassTransitHandleContext<FakeCommand>(FakeCommandHandler))
                                });

                            busFactory.Set(CommandResponseQueue);
                        }
                    }
                }

                return busFactory;
            }
        }

        public static IServiceBus CommandRequestServiceBus
        {
            get
            {
                return BusFactory.Get(CommandRequestQueue);
            }
        }

        public static IServiceBus CommandResponseServiceBus
        {
            get
            {
                return BusFactory.Get(CommandResponseQueue);
            }
        }

        public static IServiceBus EventServiceBus
        {
            get
            {
                return BusFactory.Get(EventQueue);
            }
        }

        public static FakeCommandHandler FakeCommandHandler { get; private set; }

        public static FakeEventDenormalizer FakeEventDenormalizer { get; private set; }

        #endregion
    }
}