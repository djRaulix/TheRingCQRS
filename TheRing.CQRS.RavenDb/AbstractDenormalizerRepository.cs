namespace TheRing.CQRS.RavenDb
{
    #region using

    using System;
    using System.Linq.Expressions;

    using Magnum.Reflection;

    using Raven.Abstractions.Commands;
    using Raven.Client;
    using Raven.Client.Linq;

    using TheRing.CQRS.Eventing;

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

        public void Delete<T>(object id) where T : new()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                session.Advanced.Defer(
                    new DeleteCommandData
                    {
                        Key = string.Format("{0}/{1}", this.DocumentStore.Conventions.FindTypeTagName(typeof(T)), id)
                    });
                session.SaveChanges();
            }
        }

        public void Delete<T>(Expression<Func<T, bool>> filter) where T : new()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var toDelete = session.Query<T>().Where(filter).Customize(q => q.WaitForNonStaleResults());
                foreach (var item in toDelete)
                {
                    session.Delete(item);
                }
                session.SaveChanges();
            }
        }

        public void Update<T>(object id, Action<T> update) where T : new()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var view = session.Load<T>(id.ToString());

                update(view);

                session.SaveChanges();
            }
        }

        public void Update<T>(Expression<Func<T, bool>> filter, Action<T> update) where T : new()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var toUpdate = session.Query<T>().Where(filter).Customize(q => q.WaitForNonStaleResults());

                foreach (var item in toUpdate)
                {
                    update(item);
                }
                session.SaveChanges();
            }
        }

        #endregion
    }
}