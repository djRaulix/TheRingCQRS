namespace TheRing.CQRS.Eventing
{
    using System;

    [Serializable]
    public class UnknownEventSourcedException : Exception
    {
        #region Constructors and Destructors

        public UnknownEventSourcedException(Guid eventSourcedId)
            : base(string.Format("unknown evevent sourced : {0}", eventSourcedId))
        {
        }

        #endregion
    }
}