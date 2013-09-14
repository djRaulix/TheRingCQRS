namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    #endregion

    public interface IRepository
    {
        #region Public Methods and Operators

        TAgg Create<TAgg>(Guid id) where TAgg : AggregateRoot;

        TAgg Get<TAgg>(Guid id, int? expectedVersion = null) where TAgg : AggregateRoot;

        void Save(AggregateRoot aggregateRoot);

        #endregion
    }
}