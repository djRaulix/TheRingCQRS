namespace TheRing.MassTransit.RavenDb.Test
{
    #region using

    using System;

    using Raven.Client;

    using TheRing.CQRS.Eventing.RavenDb;
    using TheRing.RavenDb;
    using TheRing.RavenDb.Test;

    #endregion

    public sealed class GlobalSpecification
    {
        #region Static Fields

        private static readonly object LockObject = new Object();
        private static IDocumentStore sagaStore;

        #endregion

        #region Public Properties

        public static IDocumentStore SagaStore
        {
            get
            {
                if (sagaStore == null)
                {
                    lock (LockObject)
                    {
                        if (sagaStore == null)
                        {
                            var documentStoreFactory = new DocumentStoreFactory(
                                new CreateInMemoryStoreStrategy());
                            var cqrsDocumentStoreFactory = new CqrsDocumentStoreFactory(
                                documentStoreFactory,
                                documentStoreFactory);
                            sagaStore = cqrsDocumentStoreFactory.SagaStore;
                        }
                    }
                }
                return sagaStore;
            }
        }

        #endregion
    }
}