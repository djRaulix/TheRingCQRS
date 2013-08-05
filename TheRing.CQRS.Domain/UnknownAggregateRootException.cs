namespace TheRing.CQRS.Domain
{
    using System;

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