namespace TheRing.CQRS.Application
{
    #region using

    using System;

    using Magnum;

    #endregion

    public abstract class AbstractCreateCommand : AbstractEditCommand
    {
        #region Fields

        private Guid id;

        #endregion

        #region Public Properties

        public override sealed int? ExpectedVersion
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public override sealed Guid Id
        {
            get
            {
                if (this.id == Guid.Empty)
                {
                    this.id = CombGuid.Generate();
                }
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        #endregion
    }
}