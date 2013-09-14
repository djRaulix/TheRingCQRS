namespace TheRing.CQRS.Domain
{
    #region using

    using System;

    #endregion

    [Serializable]
    public class NotExpectedVersionDuringLoadException : Exception
    {
        #region Constructors and Destructors

        public NotExpectedVersionDuringLoadException(Guid id, int version)
            : base(string.Format("not expectedVersion {0} was loaded for [ {1} ] Id", version, id))
        {
        }

        #endregion
    }
}