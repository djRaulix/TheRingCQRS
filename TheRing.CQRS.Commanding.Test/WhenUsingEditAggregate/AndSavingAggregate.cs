namespace TheRing.CQRS.Commanding.Test.WhenUsingEditAggregate
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    using TheRing.CQRS.Commanding.Test.Fakes;

    #endregion

    public class AndSavingAggregate : WhenUsingEditAggregate
    {
        #region Fields

        private FakeAggregateRoot editedAggregate;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenAggregateShouldBeSavedFromRepository()
        {
            this.Repository.CallsTo(r => r.Save(this.editedAggregate)).MustHaveHappened();
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.EditAggregate.Save(this.editedAggregate);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.editedAggregate = new FakeAggregateRoot();
        }

        #endregion
    }
}