namespace TheRing.CQRS.Application
{
    #region using

    using System;

    using global::TheRing.CQRS.Commanding;

    #endregion

    public abstract class AbstractEditCommand : AbstractCommand
    {
        #region Public Properties

        public virtual int? ExpectedVersion { get; set; }

        public virtual Guid Id { get; set; }

        #endregion
    }
}