namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Eventing;

    #endregion

    public class AggregateRootRepository<TAgg> : IAggregateRootRepository<TAgg>
        where TAgg : AggregateRoot
    {
        #region Fields

        private readonly IEventSourcedRepository eventSourcedRepository;

        #endregion

        #region Constructors and Destructors

        public AggregateRootRepository(IEventSourcedRepository eventSourcedRepository)
        {
            this.eventSourcedRepository = eventSourcedRepository;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create(Guid id)
        {
            return this.eventSourcedRepository.Create<TAgg>(id);
        }

        public TAgg Get(Guid id, int? expectedVersion = null)
        {
            try
            {
                return this.eventSourcedRepository.Get<TAgg>(id, expectedVersion);
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