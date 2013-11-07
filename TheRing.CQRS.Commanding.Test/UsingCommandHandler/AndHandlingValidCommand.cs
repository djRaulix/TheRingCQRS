namespace TheRing.CQRS.Commanding.Test.UsingCommandHandler
{
    #region using

    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndHandlingValidCommand : UsingCommandHandler
    {
        #region Public Methods and Operators

        [Test]
        public void ThenOkResponseShouldBeReturned()
        {
            this.Response.Ok.Should().BeTrue();
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
            this.CommandRunner.CallsTo(r => r.Run(this.Command));
        }

        #endregion
    }
}