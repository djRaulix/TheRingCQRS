namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    #endregion

    [Serializable]
    public class UnknownAggregateRootException : Exception
    {
        #region Constructors and Destructors

        public UnknownAggregateRootException(Guid aggregateRootId)
            : base(string.Format("unknown aggregate root : {0}", aggregateRootId))
        {
        }

        #endregion
    }
}