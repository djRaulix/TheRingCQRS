namespace TheRing.CQRS.Eventing.Test.UsingConcreteEventSourced
{
    #region using

    using TheRing.CQRS.Eventing.Test.Fakes;

    #endregion

    public abstract class UsingConcreteEventSourced : Specification
    {
        #region Fields

        protected FakeConcreteEventSourced EventSourced;

        #endregion

        #region Methods

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.EventSourced = new FakeConcreteEventSourced();
        }

        #endregion
    }
}