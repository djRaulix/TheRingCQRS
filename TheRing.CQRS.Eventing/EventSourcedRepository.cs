namespace TheRing.CQRS.Eventing
{
    #region using

    using System;

    #endregion

    public class EventSourcedRepository : IEventSourcedRepository

    {
        #region Fields

        private readonly IEventSourcedFactory eventSourcedFactory;

        private readonly ISnapshotKeeper keeper;
        private readonly IEventStore store;

        #endregion

        #region Constructors and Destructors

        public EventSourcedRepository(
            ISnapshotKeeper keeper,
            IEventStore store,
            IEventSourcedFactory eventSourcedFactory)
        {
            this.keeper = keeper;
            this.store = store;
            this.eventSourcedFactory = eventSourcedFactory;
        }

        #endregion

        #region Public Methods and Operators

        public TEventSourced Create<TEventSourced>(Guid id) where TEventSourced : EventSourced
        {
            var eventSourced = this.eventSourcedFactory.Create<TEventSourced>();
            eventSourced.Id = id;
            return eventSourced;
        }


        public TEventSourced Get<TEventSourced>(Guid id, int? expectedVersion = null) where TEventSourced : EventSourced
        {
            var eventSourced = this.Create<TEventSourced>(id);

            try
            {
                eventSourced.Restore(this.keeper.Get(id));
            }
            catch
            {
                this.keeper.Delete(id);
                eventSourced = this.Create<TEventSourced>(id);
            }

            eventSourced.LoadFromHistory(this.store.GetEvents(id, eventSourced.Version + 1));

            if (eventSourced.Version == 0)
            {
                throw new UnknownEventSourcedException(id);
            }

            if (expectedVersion.HasValue && eventSourced.Version != expectedVersion)
            {
                throw new NotExpectedVersionDuringLoadException(id, expectedVersion.Value);
            }

            return eventSourced;
        }

        public void Save(EventSourced eventSourced)
        {
            this.store.SaveEvents(eventSourced.Changes);

            try
            {
                this.keeper.Set(eventSourced.Id, eventSourced.Snapshot());
            }
            catch
            {
            }
        }

        #endregion
    }
}