namespace TheRing.CQRS.Commanding
{
    using System;

    public abstract class UpdateCommand : Command
    {
        #region Public Properties

        public int? ExpectedVersion { get; set; }

        public Guid Id { get; set; }

        #endregion
    }
}