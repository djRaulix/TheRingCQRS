namespace TheRing.CQRS.Commanding.Test.WhenUsingEditAggregate
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Commanding.Test.Fakes;

    #endregion

    public class AndEditingAggregateFromUpdateCommand : WhenUsingEditAggregate
    {
        #region Fields

        private UpdateCommand updateCommand;
        private FakeAggregateRoot existingAggregate;
        private FakeAggregateRoot returnAggregate;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenANewAggregateShouldBeReturned()
        {
            this.returnAggregate.Should().Be(this.existingAggregate);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.returnAggregate = this.EditAggregate.Get(this.updateCommand);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.updateCommand = A.Fake<UpdateCommand>();
            this.existingAggregate = new FakeAggregateRoot();
            this.Repository.CallsTo(r => r.Get(this.updateCommand.Id,null)).Returns(this.existingAggregate);
        }

        #endregion
    }
}