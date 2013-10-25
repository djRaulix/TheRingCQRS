namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public interface IAggregateRootRepository<TAgg>
    {
        #region Public Methods and Operators

        TAgg Create(Guid id);

        TAgg Get(Guid id, int? expectedVersion = null);

        void Save(TAgg aggregateRoot);

        #endregion
    }
}