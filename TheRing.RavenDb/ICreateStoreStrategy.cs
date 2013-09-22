namespace TheRing.RavenDb
{
    using Raven.Client;
    using Raven.Client.Document;

    public interface ICreateStoreStrategy
    {
        IDocumentStore New(DocumentConvention conventions, string defaultDatabase, string connectionStringName);
    }
}