namespace TheRing.CQRS.RavenDb

{
    using System;

    using Magnum.Reflection;

    using Raven.Client;

    using TheRing.CQRS.Eventing;

    public class DenormalizerRepository : IDenormalizerRepository
    {
        #region Fields

        protected readonly IDocumentStore DocumentStore;

        #endregion

        #region Constructors and Destructors

        public DenormalizerRepository(IDocumentStore documentStore)
        {
            this.DocumentStore = documentStore;
        }

        #endregion

        #region Public Methods and Operators

        public void Create<T>(Action<T> initialize) where T : new()
        {
            var view = FastActivator<T>.Create();

            initialize(view);

            using (var session = this.DocumentStore.OpenSession())
            {
                session.Store(view);

                session.SaveChanges();
            }
        }

        public void Update<T>(Guid id, Action<T> update) where T : new()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var view = session.Load<T>(id);

                update(view);

                session.SaveChanges();
            }
        }

        #endregion
    }
}