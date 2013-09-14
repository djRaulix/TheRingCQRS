namespace TheRing.CQRS.Eventing
{
    #region using

    using System;
    using System.Collections.Generic;

    #endregion

    public interface IEventStore
    {
        #region Public Methods and Operators

        IEnumerable<Event> GetEvents(Guid id, int fromVersion, int toVersion);

        IEnumerable<Event> GetEvents(Guid id);

        void SaveEvents(IEnumerable<Event> events);

        #endregion
    }
}