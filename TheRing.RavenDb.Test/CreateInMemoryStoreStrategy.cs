namespace TheRing.RavenDb.Test
{
    #region using

    using System;

    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;

    #endregion

    public class  CreateInMemoryStoreStrategy : ICreateStoreStrategy
    {
        #region Public Methods and Operators

        public IDocumentStore New(DocumentConvention conventions, string defaultDatabase, string connectionStringName)
        {
            return new DocumentStore()
            {
                Conventions = conventions,
                DefaultDatabase = defaultDatabase,
                Url = "http://localhost:8080"

            };
        }

        #endregion
    }
}