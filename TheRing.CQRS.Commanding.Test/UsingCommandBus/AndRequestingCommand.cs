namespace TheRing.CQRS.Commanding.Test.UsingCommandBus
{
    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Commanding.Bus;

    public class AndRequestingCommand : UsingCommandBus
    {
        private Response response;

        #region Overrides of SpecBase

        protected override void BecauseOf()
        {
            this.response = CommandBus.SendRequest(Command,CorrelationId);
        }

        #endregion

        [Test]
        public void ThenCorrelationIdShouldBeSet()
        {
            this.Command.CorrelationId.Should().Be(this.CorrelationId);
        }

        [Test]
        public void ThenExpectResponseShouldBeTrue()
        {
            this.Command.ExpectResponse.Should().BeTrue();
        }

        [Test]
        public void ThenResponseShouldBeReturned()
        {
            this.response.Should().NotBe(null);
        }
    }
}