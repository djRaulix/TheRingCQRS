namespace TheRing.CQRS.Domain.UnitTests.WhenUsingTheRepository
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Domain.UnitTests.Fakes;

    #endregion

    public class AndAskingAnUnKnownAggregate : WhenUsingTheRepository
    {
        #region Fields

        private UnknownAggregateRootException exception;

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
                this.Repository.Get<FakeAggregate>(this.Id);
            }
            catch (UnknownAggregateRootException ex)
            {
                this.exception = ex;
            }
        }

        #endregion
    }
}