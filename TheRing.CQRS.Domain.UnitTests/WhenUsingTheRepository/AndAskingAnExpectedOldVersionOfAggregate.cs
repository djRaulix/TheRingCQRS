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

    public class AndAskingAnExpectedOldVersionOfAggregate : WhenUsingTheRepository
    {
        #region Constants

        private const int ExpectedVersion = 1;

        #endregion

        #region Fields

        private NotExpectedVersionDuringLoadException exception;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAnErrorShouldBeThrown()
        {
            this.exception.Should().NotBeNull();
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();

            try
            {
                this.Repository.Get<FakeAggregate>(this.Id, ExpectedVersion);
            }
            catch (NotExpectedVersionDuringLoadException ex)
            {
                this.exception = ex;
            }
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            var event1 = A.Fake<Event>();
            event1.EventSourcedVersion = 1;
            var event2 = A.Fake<Event>();
            event2.EventSourcedVersion = 2;
            this.EventStore.CallsTo(s => s.GetEvents(this.Id)).Returns(new List<Event> { event1, event2 });
        }

        #endregion
    }
}