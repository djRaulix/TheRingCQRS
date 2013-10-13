namespace TheRing.CQRS.RavenDb
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;
    using TheRing.RavenDb;

    #endregion

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

        public IEnumerable<Event> GetEvents(Guid id)
        {
            return this.GetEvents(id, 0, int.MaxValue);
        }

        public IEnumerable<Event> GetEvents(Guid id, int fromVersion)
        {
            return this.GetEvents(id, fromVersion, int.MaxValue);
        }

        public IEnumerable<Event> GetEvents(Guid id, int fromVersion, int toVersion)
        {
            using (var session = this.documentStore.OpenSession())
            {
                var query =
                    session.Query<Event, Event_EventSourcedIdAndVersion>()
                        .Where(e => e.EventSourcedId == id)
                        .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite());
                if (fromVersion > 0)
                {
                    fromVersion--;
                    query = query.Where(m => m.EventSourcedVersion > fromVersion);
                }

                if (toVersion < int.MaxValue)
                {
                    toVersion++;
                    query = query.Where(m => m.EventSourcedVersion < toVersion);
                }

                var enumerator = session.Advanced.Stream(query);

                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Document;
                }
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
                    var id = string.Concat(
                        uncommittedEvent.EventSourcedId,
                        this.documentStore.Conventions.IdentityPartsSeparator,
                        uncommittedEvent.EventSourcedVersion);

                    session.Store(uncommittedEvent, id);
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

            foreach (var @event in events)
            {
                this.eventBus.Publish(@event);
            }
        }

        #endregion
    }
}