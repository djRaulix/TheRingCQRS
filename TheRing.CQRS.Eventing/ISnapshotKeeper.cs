namespace TheRing.CQRS.Eventing
{
    #region using

    using System;

    #endregion

    public interface ISnapshotKeeper
    {
        #region Public Methods and Operators

        void Delete(Guid id);
        object Get(Guid id);
        void Set(Guid id, object snapshot);

        #endregion
    }
}