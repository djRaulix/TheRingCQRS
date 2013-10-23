namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    #endregion

    public interface IAggregateRootRepository<TAgg> where TAgg : IAggregateRoot
    {
        #region Public Methods and Operators

        TAgg Create(Guid id);

        TAgg Get(Guid id, int? expectedVersion = null);

        void Save(TAgg aggregateRoot);

        #endregion
    }
}