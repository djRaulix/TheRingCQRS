namespace TheRing.CQRS.Domain.UnitTests.WhenUsingTheRepository
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Domain.UnitTests.Fakes;

    #endregion

    public class AndAskingANewAggregate : WhenUsingTheRepository
    {
        #region Fields

        private AggregateRoot returnedAggregate;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenANewAggregateShouldBeReturned()
        {
            this.returnedAggregate.Should().BeSameAs(this.AggregateRoot);
            this.Id.Should().Be(this.returnedAggregate.Id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.returnedAggregate = this.Repository.Create<FakeAggregate>(this.Id);
        }

        #endregion
    }
}