namespace TheRing.CQRS.MassTransit.Test.WhenUsingEventBus
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.MassTransit.Test.Fakes;
    using TheRing.CQRS.RavenDb.Test;

    #endregion

    public class AndSendingAnEvent : WhenUsingEventBus
    {
        #region Fields

        private Event @event;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenEventBusDenormalizerShouldReceiveTheEvent()
        {
            GlobalSpecification.FakeEventDenormalizer.LastConsumedEvent.CorrelationId.Should()
                .Be(this.@event.CorrelationId);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.EventBus.Publish(this.@event);
            GlobalSpecification.EventServiceBus.Endpoint.InboundTransport.Receive(c1 => c2 => { }, 1.Milliseconds());
            
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.@event = new FakeEvent();
        }

        #endregion
    }
}