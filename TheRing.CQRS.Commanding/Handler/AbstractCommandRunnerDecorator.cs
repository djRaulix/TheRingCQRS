namespace TheRing.CQRS.Commanding.Handler
{
    public abstract class AbstractCommandRunnerDecorator<TCommand> : IRunCommand<TCommand>
        where TCommand : AbstractCommand
    {
        #region Fields

        private readonly IRunCommand<TCommand> runner;

        #endregion

        #region Constructors and Destructors

        protected AbstractCommandRunnerDecorator(IRunCommand<TCommand> runner)
        {
            this.runner = runner;
        }

        #endregion

        #region Public Methods and Operators

        public virtual void Run(TCommand command)
        {
            this.runner.Run(command);
        }

        #endregion
    }
}