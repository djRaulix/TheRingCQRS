namespace TheRing.CQRS.Commanding.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;

    #endregion

    public class CommandConsumer<TCommand> : Consumes<TCommand>.Context where TCommand : class, ICommand
    {
        #region Fields

        private readonly IHandlesCommand<TCommand> commandHandler;

        #endregion

        #region Constructors and Destructors

        public CommandConsumer(IHandlesCommand<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        #endregion

        #region Public Methods and Operators

        public void Consume(IConsumeContext<TCommand> context)
        {
            this.commandHandler.Handles(new ConsumesContext(context));
        }

        #endregion

        private class ConsumesContext : IConsumesContext<TCommand>
        {
            #region Constants

            private const int NbTriesMax = 3;

            #endregion

            #region Fields

            private readonly IConsumeContext<TCommand> context;

            #endregion

            #region Constructors and Destructors

            internal ConsumesContext(IConsumeContext<TCommand> context)
            {
                this.context = context;
                this.Command = context.Message;
            }

            #endregion

            #region Public Properties

            public TCommand Command { get; private set; }

            #endregion

            #region Public Methods and Operators

            public void Respond<TResponse>(TResponse response) where TResponse : class
            {
                this.context.Respond(response);
            }

            public bool RetryLater()
            {
                if (this.context.RetryCount < NbTriesMax)
                {
                    this.context.RetryLater();
                    return true;
                }
                return false;
            }

            #endregion
        }
    }
}