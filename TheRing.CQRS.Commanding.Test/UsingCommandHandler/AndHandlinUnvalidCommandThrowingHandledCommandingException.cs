namespace TheRing.CQRS.Commanding.Test.UsingCommandHandler
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndHandlinUnvalidCommandThrowingHandledCommandingException : UsingCommandHandler
    {
        #region Fields

        private CommandingException exception;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenErrorMessageShouldBeMessageOfTheException()
        {
            this.Response.ErrorMessage.Should().Be(this.exception.Message);
        }

        [Test]
        public void ThenErrorTypeShouldBeNameTypeOfTheException()
        {
            this.Response.ErrorType.Should().Be(this.exception.GetType().Name);
        }

        [Test]
        public void ThenKoResponseShouldBeReturned()
        {
            this.Response.Ok.Should().BeFalse();
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.Response = this.CommandHandler.Handle(this.Command);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.exception = new CommandingException("handled error");
            this.CommandRunner.CallsTo(r => r.Run(this.Command)).Throws(this.exception);
        }

        #endregion
    }
}