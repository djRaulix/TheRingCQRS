namespace TheRing.RavenDb
{
    #region using

    using System.Collections.Generic;

    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    public class DocumentStoreFactory : IDocumentStoreFactory, IAddDocumenStoreFromParameters, IAddDocumenStore
    {
        #region Constants

        private const string ConnectionStringName = "RavenDbConnectionString";

        #endregion

        #region Fields

        private readonly ICreateStoreStrategy createStrategy;

        private readonly Dictionary<string, IDocumentStore> stores = new Dictionary<string, IDocumentStore>();

        #endregion

        #region Constructors and Destructors

        public DocumentStoreFactory(ICreateStoreStrategy createStrategy)
        {
            this.createStrategy = createStrategy;
        }

        #endregion

        #region Public Methods and Operators

        public void AddStore(
            DocumentStoreParameters parameters)
        {
            this.stores[parameters.DatabaseName] = this.SetStore(
                parameters);
        }

        public void AddStore(string databaseName, IDocumentStore documentStore)
        {
            this.stores[databaseName] = documentStore;
        }

        public IDocumentStore GetStore(string databaseName)
        {
            return this.stores[databaseName];
        }

        #endregion

        #region Methods

        private IDocumentStore SetStore(
            DocumentStoreParameters parameters)
        {
            var documentConvention = new DocumentConvention
            {
                JsonContractResolver = new PropertiesOnlyContractResolver(), 
                FindTypeTagName = parameters.FindTypeTagName ?? (t => t.Name), 
                FindIdentityPropertyNameFromEntityName =
                    parameters.FindIdentityPropertyNameFromEntityName ?? (entityName => "Id"), 
                CustomizeJsonSerializer = serializer => serializer.TypeNameHandling = TypeNameHandling.All, 
                TransformTypeTagNameToDocumentKeyPrefix = parameters.TransformTypeTagNameToDocumentKeyPrefix ?? (s => s)
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

            var store = this.createStrategy.New(
                documentConvention, 
                parameters.DatabaseName, 
                parameters.ConnectionStringName ?? ConnectionStringName);

            store.Initialize();

            return store;
        }

        #endregion
    }
}