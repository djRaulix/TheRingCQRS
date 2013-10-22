namespace TheRing.CQRS.Eventing
{
    using System;

    [Serializable]
    public class EventStoreConcurrencyException : Exception
    {
        #region Constructors and Destructors

        public EventStoreConcurrencyException(Guid id, int version)
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