namespace TheRing.CQRS.Commanding
{
    using System;

    public interface IUpdateCommand : ICommand
    {
        #region Public Properties

        int? ExpectedVersion { get; }

        Guid Id { get; }

        #endregion
    }
}