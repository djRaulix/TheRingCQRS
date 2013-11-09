namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingEventStore
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;

    #endregion

    public class AndSavingEvents : WhenUsingEventStore
    {
        #region Fields

        private readonly List<AbstractEvent> toStoreEvents = new List<AbstractEvent>();

        private Guid id;
        private IEnumerable<AbstractEvent> storedEvents;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAllEventsShouldBeStored()
        {
            foreach (var item in this.toStoreEvents)
            {
                this.storedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        [Test]
        public void ThenAllEventsShouldHaveBeenPublished()
        {
            foreach (var item in this.toStoreEvents)
            {
                this.EventBus.PublishedEvents.Any(e => e.EventSourcedId == item.EventSourcedId).Should().BeTrue();
            }
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.EventStore.SaveEvents(this.toStoreEvents);
            
            using (var session = this.EventDocumentStore.OpenSession())
            {
                this.storedEvents = session.Advanced.LoadStartingWith<AbstractEvent>(this.id + "/");
            }
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = Guid.NewGuid();
            for (var i = 1; i < 4; i++)
            {
                var @event = new FakeEvent { EventSourcedId = this.id, EventSourcedVersion = i };
                this.toStoreEvents.Add(@event);
            }
        }

        #endregion
    }
}