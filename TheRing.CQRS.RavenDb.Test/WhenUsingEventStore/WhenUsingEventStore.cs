namespace TheRing.CQRS.RavenDb.Test.WhenUsingEventStore
{
    #region using

    using Raven.Client;

    using TheRing.CQRS.RavenDb.Test.Fakes;

    #endregion

    public abstract class WhenUsingEventStore : Specification
    {
        #region Properties

        protected FakeEventBus EventBus { get; private set; }

        protected EventStore EventStore { get; private set; }

        protected IDocumentStore EventDocumentStore
        {
            get
            {
                return CqrsDocumentStoreFactory.EventStore;
            }
        }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();
            this.EventBus = new FakeEventBus();
            this.EventStore = new EventStore(EventDocumentStore, this.EventBus);
        }

        #endregion
    }
}