﻿namespace TheRing.CQRS.RavenDb.Eventing
{
    public interface ICqrsDocumentStoreFactoryInitializer
    {
        #region Public Methods and Operators

        void Initialize();

        void SetDocumentStoreFactory(ICqrsDocumentStoreFactory documentStoreFactory);

        #endregion
    }
}