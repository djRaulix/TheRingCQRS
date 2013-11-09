namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using Raven.Client;

    #endregion

    public abstract class WhenUsingDenormalizerRepository : Specification
    {
        #region Properties

        protected DenormalizerRepository DenormalizerRepository { get; private set; }

        protected IDocumentStore ReadModel
        {
            get
            {
                return this.CqrsDocumentStoreFactory.ReadModel;
            }
        }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();
            this.DenormalizerRepository = new DenormalizerRepository(this.ReadModel);
        }

        #endregion
    }
}