namespace TheRing.CQRS.RavenDb
{
    using System;

    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Imports.Newtonsoft.Json;

    using TheRing.CQRS.Eventing;

    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        #region Constants

        private const string ConnectionStringName = "RavenDbConnectionString";

        #endregion

        #region Constructors and Destructors

        public DocumentStoreFactory(IDocumentStoreFactoryInitializer documentStoreFactoryInitializer)
        {
            documentStoreFactoryInitializer.SetDocumentStoreFactory(this);

            this.EventStore = InitStore(type => typeof(Event).Name, "EventStore");

            this.ReadModel = InitStore(type => type.Name, "ReadModel", findIdentityPropertyNameFromEntityName: n => n + "Id");

            this.SagaStore = InitStore(type => type.Name, "SagaStore");

            documentStoreFactoryInitializer.Initialize();
        }

        #endregion

        #region Public Properties

        public IDocumentStore EventStore { get; private set; }

        public IDocumentStore ReadModel { get; private set; }

        public IDocumentStore SagaStore { get; private set; }

        #endregion

        #region Methods

        private static IDocumentStore InitStore(Func<Type, string> findTypeTagName, string databaseName, Func<string, string> findIdentityPropertyNameFromEntityName = null)
        {
            var documentConvention = new DocumentConvention
                                         {
                                             JsonContractResolver = new PropertiesOnlyContractResolver(), 
                                             FindTypeTagName = findTypeTagName, 
                                             CustomizeJsonSerializer = serializer => serializer.TypeNameHandling = TypeNameHandling.All
                                         };

            if (findIdentityPropertyNameFromEntityName != null)
            {
                documentConvention.FindIdentityPropertyNameFromEntityName = findIdentityPropertyNameFromEntityName;

                documentConvention.RegisterIdConvention<object>(
                    (dbname, commands, entity) =>
                        {
                            var entityType = entity.GetType();

                            var idProperty = documentConvention.FindIdentityPropertyNameFromEntityName(documentConvention.FindTypeTagName(entityType));

                            var property = entityType.GetProperty(idProperty);

                            return property.GetValue(entity).ToString();
                        });

                documentConvention.TransformTypeTagNameToDocumentKeyPrefix = s => string.Empty;

                documentConvention.FindFullDocumentKeyFromNonStringIdentifier = (id, type, allowNull) => id.ToString();
            }

            var store = new DocumentStore { ConnectionStringName = ConnectionStringName, Conventions = documentConvention, DefaultDatabase = databaseName };

            store.Initialize();

            return store;
        }

        #endregion
    }
}