namespace TheRing.CQRS.Eventing.EventStore
{
    using System;
    using System.Collections.Generic;

    public static class EventStoreExtensions
    {
        public static IEnumerable<AbstractEvent> Get(this IEventStore store, Guid id, int fromVersion)
        {
            return store.GetEvents(id, fromVersion, int.MaxValue);
        }

        public static IEnumerable<AbstractEvent> Get(this IEventStore store, Guid id)
        {
            return store.GetEvents(id, 1, int.MaxValue);
        }
    }
}