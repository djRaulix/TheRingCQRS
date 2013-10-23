namespace TheRing.CQRS.MassTransit.Test.WhenUsingEventBus
{
    #region using

    using TheRing.CQRS.Eventing.MassTransit;
    using TheRing.CQRS.RavenDb.Test;

    #endregion

    public class WhenUsingEventBus : Specification
    {
        #region Properties

        protected EventBus EventBus { get; private set; }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();

            this.EventBus = new EventBus(GlobalSpecification.EventServiceBus);
        }

        #endregion
    }
}