namespace TheRing.CQRS.Eventing.EventSourced.Repository.Exception
{
    using System;

    [Serializable]
    public class NewerEventSourcedVersionLoadedException : Exception
    {
        #region Constructors and Destructors

        public NewerEventSourcedVersionLoadedException(Guid id, int version)
            : base(
                string.Format(
                    "Newer version [{0}] of the event sourced with id {1} loaded",
                    version,
                    id))
        {
        }

        #endregion
    }
}