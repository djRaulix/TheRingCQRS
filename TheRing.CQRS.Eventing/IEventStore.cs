namespace TheRing.CQRS.Eventing
{
    using System;
    using System.Collections.Generic;

    public interface IEventStore
    {
        #region Public Methods and Operators

        IEnumerable<Event> GetEvents(Guid id, int fromVersion = 0, int toVersion = int.MaxValue);

        void SaveEvents(IEnumerable<Event> events);

        #endregion
    }
}