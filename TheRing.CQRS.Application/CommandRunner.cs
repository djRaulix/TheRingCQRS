namespace TheRing.CQRS.Application
{
    #region using

    using global::TheRing.CQRS.Commanding.Handler;
    using global::TheRing.CQRS.Eventing.EventSourced;
    using global::TheRing.CQRS.Eventing.EventSourced.Repository;

    #endregion

    public class CommandRunner<TEventSourced, TCommand> : IRunCommand<TCommand>
        where TEventSourced : AbstractEventSourced
        where TCommand : AbstractEditCommand
    {
        #region Fields

        private readonly IEventSourcedRepository<TEventSourced> repository;
        private readonly IRunCommand<TEventSourced, TCommand> runner;

        #endregion

        #region Constructors and Destructors

        public CommandRunner(
            IRunCommand<TEventSourced, TCommand> runner,
            IEventSourcedRepository<TEventSourced> repository)
        {
            this.runner = runner;
            this.repository = repository;
        }

        #endregion

        #region Public Methods and Operators

        public void Run(TCommand command)
        {
            var eventSourced = this.repository.Get(command.Id, command.ExpectedVersion);
            this.runner.Run(eventSourced, command);
            this.repository.Save(eventSourced);
        }

        #endregion
    }
}