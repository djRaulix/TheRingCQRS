namespace TheRing.CQRS.Eventing.RavenDb.Test
{
    #region using

    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;
    using TheRing.RavenDb;
    using TheRing.RavenDb.Test;

    #endregion

    public static class GlobalSpecification
    {
        #region Static Fields

        private static readonly object LockObject = new object();
        private static ICqrsDocumentStoreFactory cqrsDocumentStoreFactory;

        #endregion

        #region Public Properties

        public static ICqrsDocumentStoreFactory CqrsDocumentStoreFactory
        {
            get
            {
                if (cqrsDocumentStoreFactory == null)
                {
                    lock (LockObject)
                    {
                        if (cqrsDocumentStoreFactory == null)
                        {
                            var documentStoreFactory = new DocumentStoreFactory(
                                new CreateInMemoryStoreStrategy());
                            cqrsDocumentStoreFactory = new CqrsDocumentStoreFactory(
                                documentStoreFactory, 
                                documentStoreFactory, 
                                new FakeCqrsDocumentStoreFactoryInitializer());
                        }
                    }
                }

                return cqrsDocumentStoreFactory;
            }
        }

        #endregion
    }
}