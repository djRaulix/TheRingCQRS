namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public abstract class Command
    {
        #region Constructors and Destructors

        protected Command()
        {
            this.TimeStamp = DateTime.UtcNow;
        }

        #endregion

        #region Public Properties

        public Guid CorrelationId { get; private set; }

        public bool ExpectResponse { get; set; }

        public int NbTries { get; set; }

        public DateTime TimeStamp { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void SetCorrelationId(Guid id)
        {
            this.CorrelationId = id;
        }

        #endregion
    }
}