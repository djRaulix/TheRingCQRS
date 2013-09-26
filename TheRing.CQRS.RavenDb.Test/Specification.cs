namespace TheRing.CQRS.RavenDb.Test
{
    #region using

    using TheRing.Test;

    #endregion

    public abstract class Specification : SpecBase
    {
        #region Properties

        protected ICqrsDocumentStoreFactory CqrsDocumentStoreFactory { get; private set; }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();
            this.CqrsDocumentStoreFactory = GlobalSpecification.CqrsDocumentStoreFactory;
        }

        #endregion
    }
}