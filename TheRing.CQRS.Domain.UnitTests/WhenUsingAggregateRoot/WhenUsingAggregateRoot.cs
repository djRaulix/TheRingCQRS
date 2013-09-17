namespace TheRing.CQRS.Domain.UnitTests.WhenUsingAggregateRoot
{
    #region using

    using System;

    using TheRing.CQRS.Domain.UnitTests.Fakes;

    #endregion

    public abstract class WhenUsingAggregateRoot : Specification
    {
        #region Properties

        protected FakeAggregate AggregateRoot { get; set; }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.AggregateRoot = new FakeAggregate { Id = Guid.NewGuid() };
        }

        #endregion
    }
}