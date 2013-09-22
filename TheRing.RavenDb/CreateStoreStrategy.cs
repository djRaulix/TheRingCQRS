namespace TheRing.RavenDb
{
    using Raven.Client;
    using Raven.Client.Document;

    public class CreateStoreStrategy : ICreateStoreStrategy
    {
        #region Implementation of ICreateStoreStrategy

        public IDocumentStore New(DocumentConvention conventions, string defaultDatabase, string connectionStringName)
        {
            return new DocumentStore()
            {
                Conventions = conventions,
                DefaultDatabase = defaultDatabase,
                ConnectionStringName = connectionStringName
            };
        }

        #endregion
    }
}