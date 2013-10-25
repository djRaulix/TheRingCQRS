namespace TheRing.CQRS.MassTransit.Test.WhenUsingCommandBus
{
    #region using

    using TheRing.CQRS.RavenDb.Test;

    #endregion

    public class WhenUsingCommandBus : Specification
    {
        #region Properties

        protected CommandBus CommandBus { get; private set; }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();

            this.CommandBus = new CommandBus(
                GlobalSpecification.CommandResponseServiceBus,
                GlobalSpecification.CommandRequestQueue);
        }

        #endregion
    }
}