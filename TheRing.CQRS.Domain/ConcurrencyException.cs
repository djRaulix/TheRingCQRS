namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    #endregion

    /// <summary>
    ///     Occurs when there is already a newer version of the event source stored in the event store.
    /// </summary>
    [Serializable]
    public class ConcurrencyException : Exception
    {
        #region Constructors and Destructors

        public ConcurrencyException(Guid id, int version)
            : base(
                string.Format(
                    "There is a newer than {0} version of the event source with id {1} you are trying to save stored in the event store.", 
                    version, 
                    id))
        {
        }

        #endregion
    }
}