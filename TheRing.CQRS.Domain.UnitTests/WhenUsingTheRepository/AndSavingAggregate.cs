namespace TheRing.CQRS.Domain.UnitTests.WhenUsingTheRepository
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    #endregion

    public class AndSavingAggregate : WhenUsingTheRepository
    {
        #region Public Methods and Operators

        [Test]
        public void ThenEventShouldBeStored()
        {
            this.EventStore.CallsTo(s => s.SaveEvents(this.AggregateRoot.Changes)).MustHaveHappened();
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.Repository.Save(this.AggregateRoot);
        }

        #endregion
    }
}