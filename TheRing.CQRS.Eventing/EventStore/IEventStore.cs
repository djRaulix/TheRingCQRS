namespace TheRing.CQRS.Eventing.EventStore
{
    #region using

    using System;
    using System.Collections.Generic;

    #endregion

    public interface IEventStore
    {
        #region Public Methods and Operators

        IEnumerable<AbstractEvent> Get(Guid id, int fromVersion, int toVersion);

        void Save(IEnumerable<AbstractEvent> events);

        #endregion
    }
}