namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    #region using

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Runner;

    #endregion

    public class FakeCommandRunner : ICommandRunner
    {
        #region Public Methods and Operators

        public void RunGeneric(ICommand command)
        {
        }

        #endregion
    }
}