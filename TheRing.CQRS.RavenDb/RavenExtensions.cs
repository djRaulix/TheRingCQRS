namespace TheRing.CQRS.RavenDb
{
    using System.Linq;

    using Raven.Client;
    using Raven.Client.Linq;

    public static class RavenExtensions
    {
        #region Public Methods and Operators

        public static int Count<T>(this IRavenQueryable<T> results)
        {
            RavenQueryStatistics stats;
            results.Statistics(out stats).Take(0).ToList();
            return stats.TotalResults;
        }

        #endregion
    }
}