namespace TheRing.CQRS.Commanding.Test.UsingCommandBus
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndSendingCommand : UsingCommandBus
    {
        #region Public Methods and Operators

        [Test]
        public void ThenCorrelationIdShouldBeSet()
        {
            this.Command.CorrelationId.Should().Be(this.CorrelationId);
        }

        [Test]
        public void ThenExpectResponseShouldBeFalse()
        {
            this.Command.ExpectResponse.Should().BeFalse();
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.CommandBus.Send(this.Command, this.CorrelationId);
        }

        #endregion
    }
}