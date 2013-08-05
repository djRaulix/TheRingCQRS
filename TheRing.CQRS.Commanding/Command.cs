namespace TheRing.CQRS.Commanding
{
    using System;

    public abstract class Command : ICommand
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