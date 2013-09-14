namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public abstract class UpdateCommand : Command, ICommand
    {
        #region Public Properties

        public int? ExpectedVersion { get; set; }

        public Guid Id { get; set; }

        #endregion
    }
}