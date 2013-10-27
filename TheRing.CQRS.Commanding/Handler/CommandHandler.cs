namespace TheRing.CQRS.Commanding.Handler
{
    using System;

    public class CommandHandler<TCommand> : IHandleCommand<TCommand> where TCommand : AbstractCommand
    {
        #region Fields

        private readonly IRunCommand<TCommand> runner;
        private readonly IHandleException exceptionHandler;

        #endregion

        #region Constructors and Destructors

        public CommandHandler(IRunCommand<TCommand> runner, IHandleException exceptionHandler)
        {
            this.runner = runner;
            this.exceptionHandler = exceptionHandler;
        }

        #endregion

        #region Public Methods and Operators

        public void Handle(IHandleContext<TCommand> handleContext)
        {
            var command = handleContext.Command;

            try
            {
                this.runner.Run(command);
            }
            catch (Exception ex)
            {
                if (exceptionHandler.MustRetry(ex) && handleContext.RetryLater())
                {
                    return;
                }

                if (!command.ExpectResponse) throw;

                handleContext.Respond(exceptionHandler.GetMessage(ex));
                return;
            }

            if (command.ExpectResponse)
            {
                handleContext.Respond(new DoneResponse());
            }
        }

        #endregion
    }
}