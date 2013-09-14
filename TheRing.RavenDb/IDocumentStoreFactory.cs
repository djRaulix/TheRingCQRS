namespace TheRing.RavenDb
{
    #region using

    using System;

    using Raven.Client;

    #endregion

    public interface IDocumentStoreFactory
    {
        #region Public Methods and Operators

        IDocumentStore GetStore(string databaseName);

        void InitStore(
            string databaseName, 
            Func<Type, string> findTypeTagName = null, 
            Func<string, string> findIdentityPropertyNameFromEntityName = null, 
            Func<string, string> transformTypeTagNameToDocumentKeyPrefix = null);

        #endregion
    }
}