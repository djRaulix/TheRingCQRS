namespace TheRing.CQRS.Eventing.EventSourced.Snapshot
{
    #region using

    

    #endregion

    public interface IKeepSnapshot
    {
        #region Public Methods and Operators

        void Delete(ISnaphotable snaphotable);
        void Restore(ISnaphotable eventSourced);
        void Snapshot(ISnaphotable eventSourced);

        #endregion
    }
}