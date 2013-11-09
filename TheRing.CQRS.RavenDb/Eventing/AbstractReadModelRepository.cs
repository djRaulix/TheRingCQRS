namespace TheRing.CQRS.RavenDb.Eventing
{
    #region using

    using System;

    using Raven.Client;

    #endregion

    public abstract class AbstractReadModelRepository : IDisposable
    {
        #region Fields

        protected readonly IDocumentStore DocumentStore;

        #endregion

        #region Constructors and Destructors

        protected AbstractReadModelRepository(IDocumentStore documentStore)
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