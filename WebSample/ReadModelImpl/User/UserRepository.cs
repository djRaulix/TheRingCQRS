namespace WebSample.ReadModelImpl.User
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Raven.Client;

    using TheRing.CQRS.RavenDb.Eventing;

    using WebSample.ReadModel;

    #endregion

    public class UserRepository : AbstractReadModelRepository, IUserRepository
    {
        #region Constructors and Destructors

        public UserRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<UserIdentity> GetUserIdentities(
            int page,
            int nbByPage,
            out int totalNb,
            Expression<Func<UserIdentity, object>> orderByProperty = null,
            bool asc = true)
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                RavenQueryStatistics stats;
                var result = session.Query<UserView>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite())
                    .Statistics(out stats)
                    .TransformWith<UserIdentityTransformer, UserIdentity>();

                IOrderedQueryable<UserIdentity> orderedResult;

                if (orderByProperty == null)
                {
                    orderByProperty = u => u.LastName;
                }

                orderedResult = asc ? result.OrderBy(orderByProperty) : result.OrderByDescending(orderByProperty);

                var pagedResult = orderedResult
                    .Skip((page - 1)*nbByPage)
                    .Take(nbByPage)
                    .ToList();
                totalNb = stats.TotalResults;
                return pagedResult;
            }
        }

        public IEnumerable<Guid> GetAllUserIds()
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                //return only 128 first rows
                return
                    session.Query<UserView>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite())
                    .TransformWith<UserIdTransformer, UserId>().Select(u => u.Id);
            }
        } 

        public UserView GetUserView(Guid id)
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                return session.Load<UserView>(id);
            }
        }

        #endregion
    }
}