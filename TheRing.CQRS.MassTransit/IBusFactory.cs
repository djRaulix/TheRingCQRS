namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;
    using System.Collections.Generic;

    using global::MassTransit;
    using global::MassTransit.BusConfigurators;

    #endregion

    public interface IBusFactory
    {
        #region Public Methods and Operators

        IServiceBus Get(string queue);

        IServiceBus Set(
            string queue, 
            IEnumerable<KeyValuePair<Type, Func<object>>> consumers = null, 
            IEnumerable<Type> sagas = null,
            Action<ServiceBusConfigurator> moreConfig = null);

        #endregion
    }
}