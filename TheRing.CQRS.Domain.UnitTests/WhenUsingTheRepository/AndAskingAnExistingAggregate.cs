namespace TheRing.CQRS.Domain.UnitTests.WhenUsingTheRepository
{
    #region using

    using System.Collections.Generic;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Domain.UnitTests.Fakes;
    using TheRing.CQRS.Eventing;

    #endregion

    public class AndAskingAnExistingAggregate : WhenUsingTheRepository
    {
        #region Fields

        private AggregateRoot returnedAggregate;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenTheExistingAggregateShouldBeReturned()
        {
            this.returnedAggregate.Should().BeSameAs(this.AggregateRoot);
            this.Id.Should().Be(this.returnedAggregate.Id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.returnedAggregate = this.Repository.Get<FakeAggregate>(this.Id);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.EventStore.CallsTo(s => s.GetEvents(this.Id)).Returns(new List<Event> { new FakeVolatileEvent(){EventSourcedVersion = 1} });
        }

        #endregion
    }
}