namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingEventStore
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing;

    #endregion

    public class AndQueryingUnknownGuid : WhenUsingEventStore
    {
        #region Fields

        private readonly List<AbstractEvent> storedEvents = new List<AbstractEvent>();

        private Guid id;
        private IEnumerable<AbstractEvent> loadedEvents;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenShouldReturnEmptyList()
        {
            this.loadedEvents.Should().NotBeNull();
            this.loadedEvents.Count().Should().Be(0);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.loadedEvents = this.EventStore.GetEvents(this.id);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = Guid.NewGuid();
        }

        #endregion
    }
}