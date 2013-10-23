namespace TheRing.CQRS.Commanding.Test.WhenUsingEditAggregate
{
    using FakeItEasy;

    using TheRing.CQRS.Commanding.Test.Fakes;
    using TheRing.CQRS.Domain;

    public abstract class WhenUsingEditAggregate : Specification
    {
        protected EditAggregate<FakeAggregateRoot> EditAggregate { get; set; }

        protected IAggregateRootRepository<FakeAggregateRoot> Repository { get; set; }

        #region Overrides of SpecBase

        protected override void Establish_context()
        {
            base.Establish_context();
            Repository = A.Fake<IAggregateRootRepository<FakeAggregateRoot>>();
            EditAggregate = new EditAggregate<FakeAggregateRoot>(Repository);
        }

        #endregion
    }
}