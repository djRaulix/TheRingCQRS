namespace TheRing.CQRS.Commanding.Handler
{
    #region using

    using TheRing.CQRS.Commanding.Bus;

    #endregion

    public abstract class AbstractCommandHandlerDecorator<TCommand> : IHandleCommand<TCommand>
        where TCommand : AbstractCommand
    {
        #region Fields

        private readonly IHandleCommand<TCommand> handler;

        #endregion

        #region Constructors and Destructors

        protected AbstractCommandHandlerDecorator(IHandleCommand<TCommand> handler)
        {
            this.handler = handler;
        }

        #endregion

        #region Public Methods and Operators

        public virtual Response Handle(TCommand command)
        {
            return this.handler.Handle(command);
        }

        #endregion
    }
}