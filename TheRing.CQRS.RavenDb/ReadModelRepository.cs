namespace TheRing.CQRS.RavenDb
{
    #region using

    using System;

    using Raven.Client;

    #endregion

    public abstract class ReadModelRepository : IDisposable
    {
        #region Fields

        protected readonly IDocumentStore DocumentStore;

        #endregion

        #region Constructors and Destructors

        protected ReadModelRepository(IDocumentStore documentStore)
        {
            this.DocumentStore = documentStore;
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            this.DocumentStore.Dispose();
        }

        #endregion
    }
}