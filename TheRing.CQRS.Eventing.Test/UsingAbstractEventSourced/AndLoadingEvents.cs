namespace TheRing.CQRS.Eventing.Test.UsingAbstractEventSourced
{
    #region using

    using System.Linq;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndLoadingEvents : UsingAbstractEventSourced
    {
        #region Fields

        private AbstractEvent[] eventsHistory;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenEventSourcedShouldHaveSameVersionAsLastEvent()
        {
            this.EventSourced.Version.Should().Be(this.eventsHistory.Last().EventSourcedVersion);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventSourced.LoadFromHistory(this.eventsHistory);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            var firstEvent = A.Fake<AbstractEvent>();
            firstEvent.CallsTo(e => e.EventSourcedVersion).Returns(1);
            var secondEvent = A.Fake<AbstractEvent>();
            firstEvent.CallsTo(e => e.EventSourcedVersion).Returns(2);
            this.eventsHistory = new[] { firstEvent, secondEvent }
                ;
        }

        #endregion
    }
}