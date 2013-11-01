namespace TheRing.CQRS.Commanding.Handler
{
    using TheRing.CQRS.Commanding.Bus;

    public class CommandHandler<TCommand> : IHandleCommand<TCommand> where TCommand : AbstractCommand
    {
        private readonly IRunCommand<TCommand> runner;

        public CommandHandler(IRunCommand<TCommand> runner)
        {
            this.runner = runner;
        }

        #region Implementation of IHandleCommand<in TCommand>

        public Response Handle(TCommand command)
        {
            try
            {
                this.runner.Run(command);
                return new Response(true);
            }
            catch (CommandingException ex)
            {
                return new Response(ex);
            }    
        }

        #endregion
    }
}