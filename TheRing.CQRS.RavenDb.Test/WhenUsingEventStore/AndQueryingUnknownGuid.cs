namespace TheRing.CQRS.RavenDb.Test.WhenUsingEventStore
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.RavenDb.Test.Fakes;

    #endregion

    public class AndQueryingUnknownGuid : WhenUsingEventStore
    {
        #region Fields

        private readonly List<Event> storedEvents = new List<Event>();

        private Guid id;
        private IEnumerable<Event> loadedEvents;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenShouldReturnEmptyList()
        {
            loadedEvents.Should().NotBeNull();
            loadedEvents.Count().Should().Be(0);
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