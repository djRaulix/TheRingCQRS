namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Handler;

    #endregion

    public class CommandConsumer<TCommand> : Consumes<TCommand>.Context
        where TCommand : AbstractCommand
    {
        #region Fields

        private readonly IHandleCommand<TCommand> commandHandler;

        #endregion

        #region Constructors and Destructors

        public CommandConsumer(IHandleCommand<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        #endregion

        #region Public Methods and Operators

        public void Consume(IConsumeContext<TCommand> context)
        {
            var command = context.Message;
            var response = this.commandHandler.Handle(command);
            if (command.ExpectResponse)
            {
                context.Respond(response);
            }
        }

        #endregion
    }
}