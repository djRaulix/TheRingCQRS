namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    #region using

    using TheRing.CQRS.Commanding;

    #endregion

    public class FakeCommandHandler : IHandlesCommand<FakeCommand>
    {
        #region Public Properties

        public FakeCommand LastFakeCommand { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Handles(IHandlesContext<FakeCommand> commandConsummer)
        {
            this.LastFakeCommand = commandConsummer.Command;
        }

        #endregion
    }
}