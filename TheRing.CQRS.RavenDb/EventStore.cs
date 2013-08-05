namespace TheRing.CQRS.RavenDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;

    public class EventStore : IEventStore
    {
        #region Fields

        private readonly IDocumentStore documentStore;

        private readonly IEventBus eventBus;

        #endregion

        #region Constructors and Destructors

        public EventStore(IDocumentStore documentStore, IEventBus eventBus)
        {
            this.documentStore = documentStore;
            this.eventBus = eventBus;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<Event> GetEvents(Guid id, int fromVersion = 0, int toVersion = int.MaxValue)
        {
            using (var session = this.documentStore.OpenSession())
            {
                var query = session.Query<Event>().Where(e => e.EventSourcedId == id).Customize(x => x.WaitForNonStaleResults());
                if (fromVersion > 0)
                {
                    query = query.Where(m => m.EventSourcedVersion > fromVersion - 1);
                }

                if (toVersion < int.MaxValue)
                {
                    query = query.Where(m => m.EventSourcedVersion < toVersion + 1);
                }

                return query.OrderBy(e => e.EventSourcedVersion);
            }
        }

        public void SaveEvents(IEnumerable<Event> events)
        {
            events = events.ToList();

            using (var session = this.documentStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;

                foreach (var uncommittedEvent in events)
                {
                    session.Store(uncommittedEvent, string.Format("{0}/{1}", uncommittedEvent.EventSourcedId, uncommittedEvent.EventSourcedVersion));
                }

                try
                {
                    session.SaveChanges();
                }
                catch (ConcurrencyException)
                {
                    var evnt = events.First();
                    throw new ConcurrencyException(evnt.EventSourcedId, evnt.EventSourcedVersion);
                }
            }

            foreach (dynamic @event in events)
            {
                this.eventBus.Publish(@event);
            }
        }

        #endregion
    }
}