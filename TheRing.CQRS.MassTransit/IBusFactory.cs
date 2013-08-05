namespace TheRing.CQRS.MassTransit
{
    using System;
    using System.Collections.Generic;

    using global::MassTransit;

    public interface IBusFactory
    {
        #region Public Methods and Operators

        IServiceBus Get(string queue);

        IServiceBus Set(string queue, IEnumerable<KeyValuePair<Type, Func<object>>> consumers = null, IEnumerable<Type> sagas = null);

        #endregion
    }
}