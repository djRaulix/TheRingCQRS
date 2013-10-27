namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Handler;
    using TheRing.CQRS.MassTransit.Properties;

    #endregion

    public class HandleContext<TCommand> : Consumes<TCommand>.Context, IHandleContext<TCommand>
        where TCommand : AbstractCommand
    {
        #region Fields

        private readonly IHandleCommand<TCommand> commandHandler;
        private IConsumeContext<TCommand> context;

        #endregion

        #region Constructors and Destructors

        public HandleContext(IHandleCommand<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        #endregion

        #region Public Properties

        public TCommand Command
        {
            get
            {
                return this.context.Message;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Consume(IConsumeContext<TCommand> consumeContext)
        {
            this.context = consumeContext;
            this.commandHandler.Handle(this);
        }

        public void Respond<TResponse>(TResponse response) where TResponse : class
        {
            this.context.Respond(response);
        }

        public bool RetryLater()
        {
            if (this.context.RetryCount < Settings.Default.NbMaxTries)
            {
                this.context.RetryLater();
                return true;
            }
            return false;
        }

        #endregion
    }
}