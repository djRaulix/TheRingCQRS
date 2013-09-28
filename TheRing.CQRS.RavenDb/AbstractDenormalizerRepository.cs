namespace TheRing.CQRS.RavenDb
{
    #region using

    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Magnum.Linq;
    using Magnum.Reflection;

    using Raven.Abstractions.Data;
    using Raven.Client;
    using Raven.Client.Linq;
    using Raven.Json.Linq;

    using TheRing.CQRS.Eventing;
    using TheRing.RavenDb;

    #endregion

    public class AbstractDenormalizerRepository : IDenormalizerRepository
    {
        #region Fields

        protected readonly IDocumentStore DocumentStore;

        #endregion

        #region Constructors and Destructors

        protected AbstractDenormalizerRepository(IDocumentStore documentStore)
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

        public void Delete<T>(ValueType id) where T : new()
        {
            Delete<T>(id.ToString());
        }

        public void Delete<T>(string id) where T : new()
        {
            this.DocumentStore.DatabaseCommands.Delete(
                string.Concat(
                    this.DocumentStore.Conventions.FindTypeTagName(typeof(T)),
                    this.DocumentStore.Conventions.IdentityPartsSeparator,
                    id),
                null);
        }

        public void Update<T>(ValueType id, Action<T> update) where T : new()
        {
            
            using (var session = this.DocumentStore.OpenSession())
            {
                var view = session.Load<T>(id);

                update(view);

                session.SaveChanges();
            }
        }

        public void Update<T>(string id, Action<T> update) where T : new()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var view = session.Load<T>(string.Concat(
                    this.DocumentStore.Conventions.FindTypeTagName(typeof(T)),
                    this.DocumentStore.Conventions.IdentityPartsSeparator,
                    id));

                update(view);

                session.SaveChanges();
            }
        }

        #endregion
    }
}