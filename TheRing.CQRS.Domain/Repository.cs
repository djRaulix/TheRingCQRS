namespace TheRing.CQRS.Domain
{
    #region using

    using System;
    using System.Linq;

    using TheRing.CQRS.Eventing;

    #endregion

    public class Repository : IRepository
    {
        #region Fields

        private readonly IAggregateRootFactory aggregateRootFactory;

        private readonly IEventStore store;

        #endregion

        #region Constructors and Destructors

        public Repository(IEventStore store, IAggregateRootFactory aggregateRootFactory)
        {
            this.store = store;
            this.aggregateRootFactory = aggregateRootFactory;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot
        {
            var aggregateRoot = this.aggregateRootFactory.Create<TAgg>();
            aggregateRoot.Id = id;
            return aggregateRoot;
        }

        public TAgg Get<TAgg>(Guid id, int? expectedVersion = null) where TAgg : AggregateRoot
        {
            var aggregateRoot = this.Create<TAgg>(id);

            aggregateRoot.LoadFromHistory(this.store.GetEvents(id));

            if (aggregateRoot.Version == 0)
            {
                throw new UnknownAggregateRootException(id);
            }

            if (expectedVersion.HasValue && aggregateRoot.Version != expectedVersion)
            {
                throw new NotExpectedVersionDuringLoadException(id, expectedVersion.Value);
            }
            
            return aggregateRoot;
        }

        public void Save(AggregateRoot aggregateRoot)
        {
            this.store.SaveEvents(aggregateRoot.Changes);
        }

        #endregion
    }
}