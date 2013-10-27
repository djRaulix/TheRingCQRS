namespace TheRing.CQRS.Eventing.EventSourced.Repository
{
    #region using

    using System;
    using System.Linq;

    using TheRing.CQRS.Eventing.Bus;
    using TheRing.CQRS.Eventing.EventSourced.Factory;
    using TheRing.CQRS.Eventing.EventSourced.Repository.Exception;
    using TheRing.CQRS.Eventing.EventStore;

    #endregion

    public class EventSourcedRepository<TEventSourced> : IEventSourcedRepository<TEventSourced>
        where TEventSourced : AbstractEventSourced
    {
        #region Fields

        private readonly IEventSourcedFactory factory;
        private readonly IEventStore store;
        private readonly IEventBus evenBus;

        #endregion

        #region Constructors and Destructors

        public EventSourcedRepository(
            IEventSourcedFactory factory,
            IEventStore store, IEventBus evenBus)
        {
            this.factory = factory;
            this.store = store;
            this.evenBus = evenBus;
        }

        #endregion

        #region Public Methods and Operators

        public TEventSourced Create(Guid id)
        {
            var eventSourced = this.factory.New<TEventSourced>();
            eventSourced.Id = id;
            return eventSourced;
        }

        public TEventSourced Get(Guid id, int? expectedVersion = null)
        {
            var eventSourced = this.Create(id);
            if (expectedVersion == 0)
            {
                return eventSourced;
            }

            this.Refresh(eventSourced);

            if (eventSourced.Version == 0)
            {
                throw new UnknownEventSourcedException(id);
            }

            if (!expectedVersion.HasValue || eventSourced.Version == expectedVersion.Value) return eventSourced;

            if (eventSourced.Version < expectedVersion.Value)
            {
                throw new NewerEventSourcedVersionLoadedException(id, eventSourced.Version);
            }
            throw new BadEventSourcedVersionLoadedException(id, eventSourced.Version);
        }

        public void Refresh(AbstractEventSourced eventSourced)
        {
            if (eventSourced.Changes.Any())
            {
                throw new System.Exception("Can't refresh modified object");
            }
            eventSourced.LoadFromHistory(this.store.Get(eventSourced.Id, eventSourced.Version + 1));
        }

        public void Save(AbstractEventSourced eventSourced)
        {
            this.store.Save(eventSourced.Changes);
            this.evenBus.Publish(eventSourced.Changes);
        }

        #endregion
    }
}