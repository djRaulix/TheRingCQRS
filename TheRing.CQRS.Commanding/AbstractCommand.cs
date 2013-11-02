namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public abstract class AbstractCommand
    {
        #region Public Properties

        public Guid CorrelationId { get; internal set; }

        public bool ExpectResponse { get; internal set; }

        #endregion
    }
}