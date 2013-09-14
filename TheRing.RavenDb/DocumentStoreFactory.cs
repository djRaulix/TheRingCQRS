namespace TheRing.RavenDb
{
    #region using

    using System;
    using System.Collections.Generic;

    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        #region Constants

        private const string ConnectionStringName = "RavenDbConnectionString";

        #endregion

        #region Fields

        private readonly Dictionary<string, IDocumentStore> stores = new Dictionary<string, IDocumentStore>();

        #endregion

        #region Public Methods and Operators

        public IDocumentStore GetStore(string databaseName)
        {
            return this.stores[databaseName];
        }

        public void InitStore(
            string databaseName, 
            Func<Type, string> findTypeTagName = null, 
            Func<string, string> findIdentityPropertyNameFromEntityName = null, 
            Func<string, string> transformTypeTagNameToDocumentKeyPrefix = null)
        {
            this.stores[databaseName] = SetStore(
                databaseName, 
                findTypeTagName, 
                findIdentityPropertyNameFromEntityName, 
                transformTypeTagNameToDocumentKeyPrefix);
        }

        #endregion

        #region Methods

        private static IDocumentStore SetStore(
            string databaseName, 
            Func<Type, string> findTypeTagName = null, 
            Func<string, string> findIdentityPropertyNameFromEntityName = null, 
            Func<string, string> transformTypeTagNameToDocumentKeyPrefix = null)
        {
            var documentConvention = new DocumentConvention
            {
                JsonContractResolver = new PropertiesOnlyContractResolver(), 
                FindTypeTagName = findTypeTagName ?? (t => t.Name), 
                FindIdentityPropertyNameFromEntityName = findIdentityPropertyNameFromEntityName ?? (entityName => "Id"), 
                CustomizeJsonSerializer = serializer => serializer.TypeNameHandling = TypeNameHandling.All, 
                TransformTypeTagNameToDocumentKeyPrefix = transformTypeTagNameToDocumentKeyPrefix ?? (s => s)
            };
            documentConvention.DocumentKeyGenerator = (dbName, databaseCommands, entity) =>
            {
                var entityType = entity.GetType();
                var typeTagName = documentConvention.FindTypeTagName(entityType);
                var idProperty = documentConvention.FindIdentityPropertyNameFromEntityName(typeTagName);
                var property = entityType.GetProperty(idProperty);
                var id = property.GetValue(entity);
                var prefix = documentConvention.TransformTypeTagNameToDocumentKeyPrefix(typeTagName);
                return string.IsNullOrEmpty(prefix)
                    ? id.ToString()
                    : string.Concat(prefix, documentConvention.IdentityPartsSeparator, id);
            };

            documentConvention.FindFullDocumentKeyFromNonStringIdentifier = (id, type, allowNull) =>
            {
                var typeTagName = documentConvention.FindTypeTagName(type);
                var prefix = documentConvention.TransformTypeTagNameToDocumentKeyPrefix(typeTagName);
                return string.IsNullOrEmpty(prefix)
                    ? id.ToString()
                    : string.Concat(prefix, documentConvention.IdentityPartsSeparator, id);
            };

            var store = new DocumentStore
            {
                ConnectionStringName = ConnectionStringName, 
                Conventions = documentConvention, 
                DefaultDatabase = databaseName
            };

            store.Initialize();

            return store;
        }

        #endregion
    }
}