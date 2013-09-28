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

    public class AndFilteringEventsFromGuid : WhenUsingEventStore
    {
        #region Constants

        private const int Max = 7;
        private const int Min = 3;
        private const int Total = 10;

        #endregion

        #region Fields

        private readonly List<Event> storedEvents = new List<Event>();

        private IEnumerable<Event> filteredEvents;
        private IEnumerable<Event> filteredMaxEvents;
        private IEnumerable<Event> filteredMinEvents;
        private IEnumerable<Event> allEvents;
        private Guid id;

        #endregion

        #region Public Methods and Operators

        [Test]
        [Ignore("doesn't work with in memory embedded documentStore")]
        public void ThenMaxFilterShouldApply()
        {
            this.filteredMaxEvents.Count().Should().Be(7);
            foreach (var item in this.filteredMaxEvents)
            {
                this.storedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        [Test]
        [Ignore("doesn't work with in memory embedded documentStore")]
        public void ThenMinAndMaxFilterShouldApplyTogether()
        {
            this.filteredEvents.Count().Should().Be(5);
            foreach (var item in this.filteredEvents)
            {
                this.storedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        [Test]
        [Ignore("doesn't work with in memory embedded documentStore")]
        public void ThenMinFilterShouldApply()
        {
            this.filteredMinEvents.Count().Should().Be(8);
            foreach (var item in this.filteredMinEvents)
            {
                this.storedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        [Test]
        public void ThenShouldReturnAllEventsFromGuid()
        {
            this.allEvents.Count().Should().Be(10);
            foreach (var item in this.allEvents)
            {
                this.storedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.filteredEvents = this.EventStore.GetEvents(this.id, Min, Max).ToList();
            this.filteredMinEvents = this.EventStore.GetEvents(this.id, Min).ToList();
            this.filteredMaxEvents = this.EventStore.GetEvents(this.id, 0, Max).ToList();
            this.allEvents = this.EventStore.GetEvents(this.id, 0).ToList();
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            using (var session = this.EventDocumentStore.OpenSession())
            {
                this.id = Guid.NewGuid();
                for (var i = 1; i < Total + 1; i++)
                {
                    var @event = new FakeEvent { EventSourcedId = this.id, EventSourcedVersion = i };
                    session.Store(@event, string.Format("{0}/{1}", this.id, i));
                    this.storedEvents.Add(@event);
                }
                session.SaveChanges();
            }
        }

        #endregion
    }
}