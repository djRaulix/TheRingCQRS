namespace TheRing.CQRS.RavenDb.Eventing
{
    #region using

    using Raven.Client;

    #endregion

    public interface ICqrsDocumentStoreFactory
    {
        #region Public Properties

        IDocumentStore EventStore { get; }

        IDocumentStore ReadModel { get; }

        IDocumentStore SagaStore { get; }

        #endregion
    }
}