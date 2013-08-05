namespace TheRing.CQRS.Domain
{
    using System;

    public interface IRepository
    {
        #region Public Methods and Operators

        TAgg Create<TAgg>() where TAgg : AggregateRoot;

        TAgg Get<TAgg>(Guid id, int? expectedVersion = null) where TAgg : AggregateRoot;

        void Save(AggregateRoot aggregateRoot);

        #endregion
    }
}