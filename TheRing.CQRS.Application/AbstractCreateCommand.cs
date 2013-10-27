namespace TheRing.CQRS.Application
{
    using System;

    using Magnum;

    public abstract class AbstractCreateCommand : AbstractEditCommand
    {
        #region Overrides of EditCommand

        private Guid id;

        public sealed override Guid Id
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

        #region Overrides of EditCommand

        public sealed override int? ExpectedVersion
        {
            get
            {
                return 0;
            }
            set{
           
            }
        }

        #endregion

        #endregion
    }
}