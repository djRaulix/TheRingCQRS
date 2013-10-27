namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    #region using

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Context;
    using TheRing.CQRS.Commanding.Handler;

    #endregion

    public class FakeCommandHandler : IHandleCommand<FakeCommand>
    {
        #region Public Properties

        public FakeCommand LastFakeCommand { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Handle(IHandleContext<FakeCommand> commandConsummer)
        {
            this.LastFakeCommand = commandConsummer.Command;
        }

        #endregion
    }
}