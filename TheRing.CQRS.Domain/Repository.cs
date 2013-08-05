namespace TheRing.CQRS.Domain
{
    using System;
    using System.Linq;

    using Magnum;

    using TheRing.CQRS.Eventing;

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

        public TAgg Create<TAgg>() where TAgg : AggregateRoot
        {
            var aggregateRoot = this.aggregateRootFactory.Create<TAgg>();
            aggregateRoot.SetId(CombGuid.Generate());
            return aggregateRoot;
        }

        public TAgg Get<TAgg>(Guid id, int? expectedVersion = null) where TAgg : AggregateRoot
        {
            var expected = expectedVersion != null;
            var events = (expected ? this.store.GetEvents(id, toVersion: expectedVersion.Value) : this.store.GetEvents(id)).ToList();

            if (!events.Any())
            {
                throw new UnknownAggregateRootException(id);
            }

            if (expected && events.Last().EventSourcedVersion != expectedVersion)
            {
                throw new NotExpectedVersionDuringLoadException(id, expectedVersion.Value);
            }

            var aggregateRoot = this.aggregateRootFactory.Create<TAgg>();
            aggregateRoot.SetId(id);

            aggregateRoot.LoadFromHistory(events);
            return aggregateRoot;
        }

        public void Save(AggregateRoot aggregateRoot)
        {
            this.store.SaveEvents(aggregateRoot.Changes);
        }

        #endregion
    }
}