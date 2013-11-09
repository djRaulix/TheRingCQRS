namespace TheRing.CQRS.RavenDb.Eventing
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.EventStore;

    #endregion

    public class EventStore : IEventStore
    {
        #region Fields

        private readonly IDocumentStore documentStore;

        #endregion

        #region Constructors and Destructors

        public EventStore(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<AbstractEvent> Get(Guid id, int fromVersion, int toVersion)
        {
            using (var session = this.documentStore.OpenSession())
            {
                var query =
                    session.Query<AbstractEvent, AbstractEvent_EventSourcedIdAndVersion>()
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

        public void Save(IEnumerable<AbstractEvent> events)
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

                session.SaveChanges();
            }
        }

        #endregion
    }
}