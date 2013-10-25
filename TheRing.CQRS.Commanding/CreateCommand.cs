namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    using Magnum;

    #endregion

    public abstract class CreateCommand : Command, ICommand
    {
        #region Constructors and Destructors

        protected CreateCommand()
        {
            this.Id = CombGuid.Generate();
        }

        #endregion

        #region Public Properties

        public Guid Id { get; private set; }

        #endregion

        private int i;
    }
}