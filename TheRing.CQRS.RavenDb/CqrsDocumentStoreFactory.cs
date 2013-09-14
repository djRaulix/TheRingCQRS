namespace TheRing.CQRS.RavenDb
{
    #region using

    using Raven.Client;

    using TheRing.CQRS.Eventing;
    using TheRing.RavenDb;

    #endregion

    public class CqrsDocumentStoreFactory : ICqrsDocumentStoreFactory
    {
        #region Fields

        private readonly IDocumentStoreFactory documentStoreFactory;

        #endregion

        #region Constructors and Destructors

        public CqrsDocumentStoreFactory(
            IDocumentStoreFactory documentStoreFactory, 
            ICqrsDocumentStoreFactoryInitializer initializer)
        {
            this.documentStoreFactory = documentStoreFactory;
            documentStoreFactory.InitStore("EventStore", type => typeof(Event).Name, null, s => null);

            documentStoreFactory.InitStore("ReadModel", null, n => n + "Id");

            documentStoreFactory.InitStore("SagaStore");

            initializer.SetDocumentStoreFactory(this);
            initializer.Initialize();
        }

        #endregion

        #region Public Properties

        public IDocumentStore EventStore
        {
            get { return this.documentStoreFactory.GetStore("EventStore"); }
        }

        public IDocumentStore ReadModel
        {
            get { return this.documentStoreFactory.GetStore("ReadModel"); }
        }

        public IDocumentStore SagaStore
        {
            get { return this.documentStoreFactory.GetStore("SagaStore"); }
        }

        #endregion
    }
}