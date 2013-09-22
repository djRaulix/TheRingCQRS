namespace TheRing.CQRS.Domain.UnitTests.WhenUsingAggregateRoot
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Domain.UnitTests.Fakes;
    using TheRing.CQRS.Eventing;

    #endregion

    public class AndLoadingEvents : WhenUsingAggregateRoot
    {
        #region Fields

        private const int FinalVersion = 5;

        private readonly List<Event> events = new List<Event>();

        private const string PropertyValue = "value";

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenTheAggregateShouldHaveSameVersionAsTheLastEvent()
        {
            var lastEvent = events.Last();
            FinalVersion.Should().Be(lastEvent.EventSourcedVersion);
            AggregateRoot.Version.Should().Be(lastEvent.EventSourcedVersion);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.AggregateRoot.LoadFromHistory(events);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            for (var i = 0; i < FinalVersion; i++)
            {
                events.Add(new FakeVolatileEvent() { EventSourcedVersion = i + 1 });
            }
        }

        #endregion
    }
}