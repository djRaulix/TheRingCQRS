namespace TheRing.CQRS.Eventing
{
    using System;

    public class NoSnapshotKeeper : ISnapshotKeeper
    {
        #region Public Methods and Operators

        public void Delete(Guid id)
        {
        }

        public object Get(Guid id)
        {
            return null;
        }

        public void Set(Guid id, object snapshot)
        {
        }

        #endregion
    }
}