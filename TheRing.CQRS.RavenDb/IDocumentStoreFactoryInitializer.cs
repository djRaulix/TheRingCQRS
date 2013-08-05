namespace TheRing.CQRS.RavenDb
{
    public interface IDocumentStoreFactoryInitializer
    {
        #region Public Methods and Operators

        void Initialize();

        void SetDocumentStoreFactory(IDocumentStoreFactory documentStoreFactory);

        #endregion
    }
}