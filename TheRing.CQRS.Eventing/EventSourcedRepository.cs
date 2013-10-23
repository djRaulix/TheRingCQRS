namespace TheRing.CQRS.Eventing
{
    #region using

    using System;

    #endregion

    public class EventSourcedRepository<TEventSourced> : IEventSourcedRepository<TEventSourced>
        where TEventSourced : EventSourced
    {
        #region Fields

        private readonly IEventSourcedFactory eventSourcedFactory;

        private readonly IEventStore store;

        #endregion

        #region Constructors and Destructors

        public EventSourcedRepository(IEventStore store, IEventSourcedFactory eventSourcedFactory)
        {
            this.store = store;
            this.eventSourcedFactory = eventSourcedFactory;
        }

        #endregion

        #region Public Methods and Operators

        public TEventSourced Create(Guid id)
        {
            var eventSourced = this.eventSourcedFactory.Create<TEventSourced>();
            eventSourced.Id = id;
            return eventSourced;
        }


        public TEventSourced Get(Guid id, int? expectedVersion = null)
        {
            var aggregateRoot = this.Create(id);

            aggregateRoot.LoadFromHistory(this.store.GetEvents(id));

            if (aggregateRoot.Version == 0)
            {
                throw new UnknownEventSourcedException(id);
            }

            if (expectedVersion.HasValue && aggregateRoot.Version != expectedVersion)
            {
                throw new NotExpectedVersionDuringLoadException(id, expectedVersion.Value);
            }

            return aggregateRoot;
        }

        public void Save(EventSourced eventSourced)
        {
            this.store.SaveEvents(eventSourced.Changes);
        }

        #endregion
    }
}