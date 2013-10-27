namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;
    using System.Collections.Generic;

    using global::MassTransit;
    using global::MassTransit.BusConfigurators;

    using TheRing.CQRS.MassTransit.Properties;

    #endregion

    public static class BusFactoryExtensions
    {
        #region Public Methods and Operators

        public static void InitCommanding(
            this IBusFactory factory,
            IEnumerable<KeyValuePair<Type, Func<object>>> commandConsumers,
            Action<ServiceBusConfigurator> moreConfig = null)
        {
            factory.Set(Settings.Default.RequestQueue, commandConsumers, moreConfig: moreConfig);

            factory.Set(Settings.Default.ResponseQueue, moreConfig: moreConfig);
        }

        public static void InitEventing(
            this IBusFactory factory,
            IEnumerable<KeyValuePair<Type, Func<object>>> eventConsumers,
            Action<ServiceBusConfigurator> moreConfig = null)
        {
            factory.Set(Settings.Default.ReadModelQueue, eventConsumers, moreConfig: moreConfig);
        }

        public static void InitSagas(
            this IBusFactory factory,
            IEnumerable<Type> sagas = null,
            Action<ServiceBusConfigurator> moreConfig = null)
        {
            factory.Set(Settings.Default.SagaQueue, sagas: sagas, moreConfig: moreConfig);
        }

        public static IServiceBus ReadModelQueue(this IBusFactory factory)
        {
            return factory.Get(Settings.Default.ReadModelQueue);
        }

        public static IServiceBus RequestQueue(this IBusFactory factory)
        {
            return factory.Get(Settings.Default.RequestQueue);
        }

        public static IServiceBus ResponseQueue(this IBusFactory factory)
        {
            return factory.Get(Settings.Default.ResponseQueue);
        }

        public static IServiceBus SagaQueue(this IBusFactory factory)
        {
            return factory.Get(Settings.Default.SagaQueue);
        }

        #endregion
    }
}