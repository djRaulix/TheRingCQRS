namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Runner;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.EventSourced;

    #endregion

    public class AggregateRootRepository<TAgg> : ICommandRunnerRepository<TAgg>
        where TAgg : AbstractAggregateRoot
    {
        #region Fields

        private readonly IEventSourcedRepository<TAgg> eventSourcedRepository;

        #endregion

        #region Constructors and Destructors

        public AggregateRootRepository(IEventSourcedRepository<TAgg> eventSourcedRepository)
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
                throw new CommandRunnerConcurrencyException(ex.Message);
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
                throw new CommandRunnerConcurrencyException(ex.Message);
            }
        }

        #endregion
    }
}