namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingEventStore
{
    #region using

    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    using Raven.Abstractions.Exceptions;

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;

    #endregion

    public class AndSavingConcurrencyEvents : WhenUsingEventStore
    {
        #region Fields

        private readonly List<AbstractEvent> toStoreEvents = new List<AbstractEvent>();

        private ConcurrencyException concurrencyException;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenConcurrencyExceptionShouldBeThrown()
        {
            this.concurrencyException.Should().NotBeNull();
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.EventStore.SaveEvents(this.toStoreEvents);

            try
            {
                this.EventStore.SaveEvents(this.toStoreEvents);
            }
            catch (ConcurrencyException ex)
            {
                this.concurrencyException = ex;
            }
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            for (var i = 1; i < 4; i++)
            {
                var @event = new FakeEvent { EventSourcedId = Guid.NewGuid(), EventSourcedVersion = i };
                this.toStoreEvents.Add(@event);
            }
        }

        #endregion
    }
}