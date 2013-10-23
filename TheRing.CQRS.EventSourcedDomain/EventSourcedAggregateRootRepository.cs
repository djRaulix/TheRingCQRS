namespace TheRing.CQRS.EventSourcedDomain
{
    #region using

    using System;

    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;

    #endregion

    public class EventSourcedAggregateRootRepository<TAgg> : IAggregateRootRepository<TAgg>
        where TAgg : EventSourcedAggregateRoot
    {
        #region Fields

        private readonly IEventSourcedRepository<TAgg> eventSourcedRepository;

        #endregion

        #region Constructors and Destructors

        public EventSourcedAggregateRootRepository(IEventSourcedRepository<TAgg> eventSourcedRepository)
        {
            this.eventSourcedRepository = eventSourcedRepository;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create(Guid id)
        {
            return this.eventSourcedRepository.Create(id);
        }

        public TAgg Get(Guid id, int? expectedVersion = null)
        {
            try
            {
                return this.eventSourcedRepository.Get(id, expectedVersion);
            }
            catch (EventStoreConcurrencyException ex)
            {
                throw new AggregateRootConcurrencyException(ex.Message);
            }
        }

        public void Save(TAgg aggregateRoot)
        {
            try
            {
                this.eventSourcedRepository.Save(aggregateRoot);
            }
            catch (EventStoreConcurrencyException ex)
            {
                throw new AggregateRootConcurrencyException(ex.Message);
            }
        }

        #endregion
    }
}