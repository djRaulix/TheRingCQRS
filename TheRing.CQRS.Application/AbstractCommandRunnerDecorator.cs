namespace TheRing.CQRS.Application
{
    #region using

    #endregion

    namespace TheRing.CQRS.Commanding.Handler
    {
        #region using

        using global::TheRing.CQRS.Commanding.Handler;
        using global::TheRing.CQRS.Eventing.EventSourced;

        #endregion

        public abstract class AbstractCommandRunnerDecorator<TEventSourced, TCommand> : IRunCommand<TEventSourced, TCommand>
            where TCommand : AbstractEditCommand
            where TEventSourced : AbstractEventSourced
        {
            #region Fields

            private readonly IRunCommand<TEventSourced,TCommand> runner;

            #endregion

            #region Constructors and Destructors

            protected AbstractCommandRunnerDecorator(IRunCommand<TEventSourced,TCommand> runner)
            {
                this.runner = runner;
            }

            #endregion

            #region Public Methods and Operators

            public virtual void Run(TEventSourced eventSourced, TCommand command)
            {
                this.runner.Run(eventSourced, command);
            }

            #endregion
        }
    }

}