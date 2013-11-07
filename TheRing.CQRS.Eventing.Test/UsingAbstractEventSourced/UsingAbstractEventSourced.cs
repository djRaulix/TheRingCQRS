namespace TheRing.CQRS.Eventing.Test.UsingAbstractEventSourced
{
    using FakeItEasy;

    using TheRing.CQRS.Eventing.EventSourced;

    public abstract class UsingAbstractEventSourced : Specification
    {
        protected AbstractEventSourced EventSourced;

        #region Overrides of SpecBase

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventSourced = A.Fake<AbstractEventSourced>();
        }

        #endregion
    }
}
