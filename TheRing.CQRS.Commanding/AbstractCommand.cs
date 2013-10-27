namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public abstract class AbstractCommand
    {
        #region Public Properties

        public Guid CorrelationId { get; set; }

        public bool ExpectResponse { get; set; }

        #endregion
    }
}