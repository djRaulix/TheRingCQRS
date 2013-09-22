namespace TheRing.RavenDb
{
    using Raven.Client;

    public interface IAddDocumenStore
    {
        void AddStore(string databaseName, IDocumentStore documentStore);
    }
}