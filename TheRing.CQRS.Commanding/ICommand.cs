namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public interface ICommand
    {
        #region Public Properties

        Guid CorrelationId { get; }

        bool ExpectResponse { get; set; }

        Guid Id { get; }

        DateTime TimeStamp { get; }

        #endregion

        #region Public Methods and Operators

        void SetCorrelationId(Guid id);

        #endregion
    }
}