namespace TheRing.MassTransit.RavenDb.Test
{
    #region using

    using Raven.Client;

    using TheRing.Test;

    #endregion

    public abstract class Specification : SpecBase
    {
        #region Properties

        protected IDocumentStore SagaStore { get; private set; }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();
            this.SagaStore = GlobalSpecification.SagaStore;
        }

        #endregion
    }
}