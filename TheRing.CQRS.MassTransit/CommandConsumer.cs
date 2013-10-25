namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;

    #endregion

    public class CommandConsumer<TCommand> : Consumes<TCommand>.Context, IHandlesContext<TCommand>
        where TCommand : class, ICommand
    {
        #region Constants

        private const int NbMaxTries = 3;

        #endregion

        #region Fields

        private readonly IHandlesCommand<TCommand> commandHandler;
        private IConsumeContext<TCommand> context;

        #endregion

        #region Constructors and Destructors

        public CommandConsumer(IHandlesCommand<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        #endregion

        #region Public Properties

        public TCommand Command { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Consume(IConsumeContext<TCommand> consumeContext)
        {
            this.context = consumeContext;
            this.Command = this.context.Message;
            this.commandHandler.Handles(this);
        }

        public void Respond<TResponse>(TResponse response) where TResponse : class
        {
            this.context.Respond(response);
        }

        public bool RetryLater()
        {
            if (this.context.RetryCount < NbMaxTries)
            {
                this.context.RetryLater();
                return true;
            }
            return false;
        }

        #endregion
    }
}