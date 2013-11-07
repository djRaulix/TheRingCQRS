namespace TheRing.CQRS.Commanding.Test.UsingAbstractCommandHandlerDecorator
{
    #region using

    using FakeItEasy;

    using TheRing.CQRS.Commanding.Handler;
    using TheRing.CQRS.Commanding.Test.Fakes;

    #endregion

    public abstract class UsingAbstractCommandHandlerDecorator : Specification
    {
        #region Fields

        protected FakeCommand Command;
        protected IHandleCommand<FakeCommand> CommandHandler;
        protected AbstractCommandHandlerDecorator<FakeCommand> Decorator;

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.CommandHandler = A.Fake<IHandleCommand<FakeCommand>>();
            this.Decorator =
                A.Fake<AbstractCommandHandlerDecorator<FakeCommand>>(
                    d => d.WithArgumentsForConstructor(new[] { this.CommandHandler }));
        }

        #endregion
    }
}