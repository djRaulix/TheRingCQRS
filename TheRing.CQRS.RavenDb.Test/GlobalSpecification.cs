namespace TheRing.CQRS.RavenDb.Test
{
    #region using

    using System;

    using TheRing.CQRS.RavenDb.Test.Fakes;
    using TheRing.RavenDb;
    using TheRing.RavenDb.Test;

    #endregion

    public sealed class GlobalSpecification
    {
        #region Static Fields

        private static readonly object LockObject = new Object();
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