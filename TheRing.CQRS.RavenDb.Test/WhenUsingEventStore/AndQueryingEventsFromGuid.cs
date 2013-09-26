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

    public class AndQueryingEventsFromGuid : WhenUsingEventStore
    {
        #region Fields

        private readonly List<Event> storedEvents = new List<Event>();

        private Guid id;
        private IEnumerable<Event> loadedEvents;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAllGuidRelatedEventsShouldBeReturned()
        {
            foreach (var item in this.storedEvents.Where(e => e.EventSourcedId == this.id))
            {
                this.loadedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        [Test]
        public void ThenGuidNonRelatedEventsShouldNotBeReturned()
        {
            foreach (var item in this.storedEvents.Where(e => e.EventSourcedId != this.id))
            {
                this.loadedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeFalse();
            }
        }

        [Test]
        public void ThenReturnedEventListShouldBeSortedAndContiguous()
        {
            var version = 0;
            foreach (var item in this.loadedEvents)
            {
                version++;
                item.EventSourcedVersion.Should().Be(version);
            }
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
            using (var session = this.EventDocumentStore.OpenSession())
            {
                Event @event;
                this.id = Guid.NewGuid();
                for (var i = 1; i < 4; i++)
                {
                    @event = new FakeEvent { EventSourcedId = this.id, EventSourcedVersion = i };
                    session.Store(@event, string.Format("{0}/{1}", this.id, i));
                    this.storedEvents.Add(@event);
                }
                @event = new FakeEvent { EventSourcedId = Guid.NewGuid(), EventSourcedVersion = 1 };
                session.Store(@event, string.Format("{0}/{1}", @event.EventSourcedId, 1));
                this.storedEvents.Add(@event);

                session.SaveChanges();
            }
        }

        #endregion
    }
}