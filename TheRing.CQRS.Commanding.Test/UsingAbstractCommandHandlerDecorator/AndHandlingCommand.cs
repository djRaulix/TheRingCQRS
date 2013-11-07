namespace TheRing.CQRS.Commanding.Test.UsingAbstractCommandHandlerDecorator
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Commanding.Bus;

    #endregion

    public class AndHandlingCommand : UsingAbstractCommandHandlerDecorator
    {
        #region Fields

        private Response DecoratorResponse;
        private Response HandlerResponse;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenShouldReturnDecoratedHandlerResponse()
        {
            this.DecoratorResponse.Should().Be(this.HandlerResponse);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.DecoratorResponse = this.Decorator.Handle(this.Command);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.HandlerResponse = new Response(true);
            this.CommandHandler.CallsTo(h => h.Handle(this.Command)).Returns(this.HandlerResponse);
            this.Decorator.CallsTo(d => d.Handle(this.Command)).CallsBaseMethod();
        }

        #endregion
    }
}