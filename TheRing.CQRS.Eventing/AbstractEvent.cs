namespace TheRing.CQRS.Eventing
{
    #region using

    using System;

    #endregion

    public abstract class AbstractEvent
    {
        #region Public Properties

        public Guid CorrelationId { get; internal set; }

        public Guid EventSourcedId { get; internal set; }

        public int EventSourcedVersion { get; internal set; }

        public DateTime TimeStamp { get; internal set; }

        #endregion
    }
}