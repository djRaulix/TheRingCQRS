namespace WebSample.App_Start
{
    #region using

    using System.Linq;

    using Magnum.Reflection;

    using Raven.Client.Indexes;

    using TheRing.CQRS.RavenDb.Eventing;

    #endregion

    public class CqrsDocumentStoreFactoryInitializer : ICqrsDocumentStoreFactoryInitializer
    {
        #region Fields

        private ICqrsDocumentStoreFactory factory;

        #endregion

        #region Public Methods and Operators

        public void Initialize()
        {
            this.RegisterReadModelIndexes();
        }

        public void SetDocumentStoreFactory(ICqrsDocumentStoreFactory documentStoreFactory)
        {
            this.factory = documentStoreFactory;
        }

        #endregion

        #region Methods

        private void RegisterReadModelIndexes()
        {
            /*var indexes = from @type in this.GetType().Assembly.GetTypes()
                where
                    @type.Namespace != null && @type.Namespace.Contains("ReadModel") && !@type.IsAbstract &&
                    typeof(AbstractIndexCreationTask).IsAssignableFrom(@type)
                select @type;

            foreach (var task in indexes.Select(index => (AbstractIndexCreationTask)FastActivator.Create(index)))
            {
                task.Execute(this.factory.ReadModel);
            }*/

            var transformers = from @type in this.GetType().Assembly.GetTypes()
                where
                    !@type.IsAbstract &&
                    typeof(AbstractTransformerCreationTask).IsAssignableFrom(@type)
                select @type;

            foreach (
                var task in
                    transformers.Select(
                        transformer => (AbstractTransformerCreationTask)FastActivator.Create(transformer)))
            {
                task.Execute(this.factory.ReadModel);
            }
        }

        #endregion
    }
}