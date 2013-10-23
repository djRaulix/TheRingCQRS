namespace TheRing.CQRS.Commanding.Test.WhenUsingEditAggregate
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Commanding.Test.Fakes;

    #endregion

    public class AndEditingAggregateFromCreateCommand : WhenUsingEditAggregate
    {
        #region Fields

        private CreateCommand createCommand;
        private FakeAggregateRoot newAggregate;
        private FakeAggregateRoot returnAggregate;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenANewAggregateShouldBeReturned()
        {
            this.returnAggregate.Should().Be(this.newAggregate);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.returnAggregate = this.EditAggregate.Get(this.createCommand);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.createCommand = A.Fake<CreateCommand>();
            this.newAggregate = new FakeAggregateRoot();
            this.Repository.CallsTo(r => r.Create(this.createCommand.Id)).Returns(this.newAggregate);
        }

        #endregion
    }
}