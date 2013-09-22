namespace TheRing.CQRS.Domain.UnitTests.WhenUsingAggregateRoot
{
    #region using

    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Domain.UnitTests.Fakes;
    using TheRing.CQRS.Eventing;

    #endregion

    public class AndLoadingAVolatileEvent : WhenUsingAggregateRoot
    {
        #region Constants

        private const string PropertyValue = "value";

        #endregion

        #region Fields

        private FakeVolatileEvent @event;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenTheAggregateShouldNotApllyTheEvent()
        {
            this.AggregateRoot.VolatileProperty.Should().NotBe(PropertyValue);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.@event.Property = PropertyValue;
            this.AggregateRoot.LoadFromHistory(new List<Event> { this.@event });
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.@event = new FakeVolatileEvent();
        }

        #endregion
    }
}