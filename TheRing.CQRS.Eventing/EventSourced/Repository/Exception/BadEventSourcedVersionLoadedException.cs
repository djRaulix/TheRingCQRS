namespace TheRing.CQRS.Eventing.EventSourced.Repository.Exception
{
    using System;

    [Serializable]
    public class BadEventSourcedVersionLoadedException : Exception
    {
        #region Constructors and Destructors

        public BadEventSourcedVersionLoadedException(Guid id, int version)
            : base(
                string.Format(
                    "Bad version [{0}] of the event sourced with id {1} loaded",
                    version,
                    id))
        {
        }

        #endregion
    }
}