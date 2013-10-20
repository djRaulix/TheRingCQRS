namespace WebSample.App_Start
{
    using System.Linq;

    using TheRing.CQRS.RavenDb;

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
        }

        #endregion
    }
}