namespace WebSample.ReadModel
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Client;

    using TheRing.CQRS.RavenDb;

    #endregion

    public class UserRepository : ReadModelRepository
    {
        #region Constructors and Destructors

        public UserRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<UserIdentity> GetUserIdentities(int page, int nbByPage, out int totalNb)
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                RavenQueryStatistics stats;
                var results = session.Query<UserView>()
                    .Statistics(out stats)
                    .TransformWith<UserIdentityTransformer,UserIdentity>()
                    .Skip((page - 1) * nbByPage)
                    .Take(nbByPage) 
                    .ToList();
                totalNb = stats.TotalResults;
                return results;
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