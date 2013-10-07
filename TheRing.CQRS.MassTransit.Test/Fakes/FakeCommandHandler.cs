namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    using TheRing.CQRS.Commanding;

    public class FakeCommandHandler : IRunCommand<FakeAggregateRoot, FakeCommand>
    {
        #region Implementation of IRunCommand<in FakeAggregateRoot,in FakeCommand>

        public FakeCommand LastFakeCommand
        {
            get;
            private set;
        }

        public void Run(FakeAggregateRoot aggregateRoot, FakeCommand command)
        {
            LastFakeCommand = command;
        }

        #endregion
    }
}