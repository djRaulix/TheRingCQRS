namespace TheRing.CQRS.Commanding
{
    using System;

    public interface ICommand
    {
        #region Public Properties

        Guid CorrelationId { get; }

        bool ExpectResponse { get; set; }

        DateTime TimeStamp { get; }

        #endregion

        #region Public Methods and Operators

        void SetCorrelationId(Guid id);

        #endregion
    }
}