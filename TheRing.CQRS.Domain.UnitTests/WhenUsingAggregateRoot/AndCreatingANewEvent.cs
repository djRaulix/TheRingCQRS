namespace TheRing.CQRS.Domain.UnitTests.WhenUsingAggregateRoot
{
    #region using

    using System;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Domain.UnitTests.Fakes;
    using TheRing.CQRS.Eventing;

    #endregion

    public class AndCreatingANewEvent : WhenUsingAggregateRoot
    {
        #region Fields

        private Event @event;
        private int initVersion;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAggregateRootVersionShouldBeIncremented()
        {
            this.AggregateRoot.Version.Should().Be(this.initVersion + 1);
        }

        [Test]
        public void ThenTheEventShouldBeAddedToTheChangesList()
        {
            this.AggregateRoot.Changes.Should().Contain(this.@event);
        }

        [Test]
        public void ThenTheEventShouldHaveAggregateRootIdAndCurrentVersion()
        {
            this.@event.EventSourcedId.Should().NotBe(Guid.Empty);
            this.@event.EventSourcedId.Should().Be(this.AggregateRoot.Id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.initVersion = this.AggregateRoot.Version;
            this.AggregateRoot.ApplyChangeInPublicMethod(this.@event);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.@event = new FakeNonVolatileEvent();       
        }

        #endregion
    }
}