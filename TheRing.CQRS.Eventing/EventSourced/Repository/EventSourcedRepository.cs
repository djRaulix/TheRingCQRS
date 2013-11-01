namespace TheRing.CQRS.Eventing.EventSourced.Repository
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;

    using TheRing.CQRS.Eventing.Bus;
    using TheRing.CQRS.Eventing.EventSourced.Factory;
    using TheRing.CQRS.Eventing.EventSourced.Snapshot;
    using TheRing.CQRS.Eventing.EventStore;

    #endregion

    public class EventSourcedRepository : IEventSourcedRepository
    {
        #region Fields

        private readonly IEventBus eventBus;
        private readonly IEventStore eventStore;
        private readonly IEventSourcedFactory factory;
        private readonly IKeepSnapshot snapshotKeeper;

        #endregion

        #region Constructors and Destructors

        public EventSourcedRepository(
            IEventSourcedFactory factory,
            IEventStore eventStore,
            IKeepSnapshot snapshotKeeper,
            IEventBus eventBus)
        {
            this.factory = factory;
            this.eventStore = eventStore;
            this.snapshotKeeper = snapshotKeeper;
            this.eventBus = eventBus;
        }

        #endregion

        #region Public Methods and Operators

        public TEventSourced Create<TEventSourced>(Guid id) where TEventSourced : AbstractEventSourced
        {
            var eventSourced = this.factory.New<TEventSourced>();
            eventSourced.Id = id;
            return eventSourced;
        }

        public TEventSourced Get<TEventSourced>(Guid id) where TEventSourced : AbstractEventSourced
        {
            var eventSourced = this.Create<TEventSourced>(id);
            var snapshotable = eventSourced as ISnaphotable;
            if (snapshotable != null)
            {
                try
                {
                    this.snapshotKeeper.Restore(snapshotable);
                }
                catch
                {
                    this.snapshotKeeper.Delete(snapshotable);
                    eventSourced = this.Create<TEventSourced>(id);
                }
            }

            eventSourced.LoadFromHistory(this.eventStore.Get(id, eventSourced.Version + 1));

            if (eventSourced.Version == 0)
            {
                throw new UnKnownEventSourcedException(id);
            }

            return eventSourced;
        }

        public void Save(AbstractEventSourced eventSourced)
        {
            this.eventStore.Save(eventSourced.Changes);
            this.eventBus.Publish(eventSourced.Changes);

            var snapshotable = eventSourced as ISnaphotable;
            if (snapshotable == null) return;

            try
            {
                this.snapshotKeeper.Snapshot(snapshotable);
            }
            catch
            {
            }
        }
        #endregion
    }
}