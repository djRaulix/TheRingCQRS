namespace TheRing.RavenDb
{
    #region using

    using System;
    using System.Collections.Specialized;

    using Raven.Client;

    #endregion

    public interface IDocumentStoreFactory
    {
        #region Public Methods and Operators

        IDocumentStore GetStore(string databaseName);

        #endregion
    }
}