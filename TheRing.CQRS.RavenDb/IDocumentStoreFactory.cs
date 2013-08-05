namespace TheRing.CQRS.RavenDb
{
    using Raven.Client;

    public interface IDocumentStoreFactory
    {
        #region Public Properties

        IDocumentStore EventStore { get; }

        IDocumentStore ReadModel { get; }

        IDocumentStore SagaStore { get; }

        #endregion
    }
}