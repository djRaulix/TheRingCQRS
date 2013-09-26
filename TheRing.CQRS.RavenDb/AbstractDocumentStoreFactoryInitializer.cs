namespace TheRing.CQRS.RavenDb
{
    public abstract class AbstractDocumentStoreFactoryInitializer : ICqrsDocumentStoreFactoryInitializer
    {
        #region Properties

        protected ICqrsDocumentStoreFactory CqrsDocumentStoreFactory { get; private set; }

        #endregion

        #region Public Methods and Operators

        public virtual void Initialize()
        {
        }

        public void SetDocumentStoreFactory(ICqrsDocumentStoreFactory documentStoreFactory)
        {
            this.CqrsDocumentStoreFactory = documentStoreFactory;
        }

        #endregion
    }
}