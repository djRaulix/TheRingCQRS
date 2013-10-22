namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    #endregion

    /// <summary>
    ///     Occurs when there is already a newer version of the event source stored in the event store.
    /// </summary>
    [Serializable]
    public class DomainConcurrencyException : Exception
    {
        #region Constructors and Destructors

        public DomainConcurrencyException(string message)
            : base(message)
        {
        }

        #endregion
    }
}