namespace TheRing.RavenDb
{
    #region using

    using System;

    using Raven.Client;

    #endregion

    public class DocumentStoreParameters
    {
        #region Public Properties

        public string ConnectionStringName { get; set; }

        public string DatabaseName { get; set; }

        public Func<string, string> FindIdentityPropertyNameFromEntityName { get; set; }

        public Func<Type, string> FindTypeTagName { get; set; }

        public Func<string, string> TransformTypeTagNameToDocumentKeyPrefix { get; set; }

        #endregion
    }
}