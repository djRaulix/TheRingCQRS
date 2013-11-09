namespace TheRing.CQRS.Eventing.RavenDb.Test.Fakes
{
    public class FakeCqrsDocumentStoreFactoryInitializer : AbstractDocumentStoreFactoryInitializer
    {
        #region Public Properties

        public static ICqrsDocumentStoreFactory DocumentStoreFactory { get; private set; }

        public static bool Initialized { get; private set; }

        #endregion

        #region Public Methods and Operators

        public override void Initialize()
        {
            Initialized = true;
            DocumentStoreFactory = this.CqrsDocumentStoreFactory;
        }

        #endregion
    }
}