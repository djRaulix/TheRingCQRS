namespace TheRing.RavenDb
{
    #region using

    using System.Collections.Generic;
    using System.Linq;

    using Raven.Abstractions.Data;
    using Raven.Client;
    using Raven.Client.Linq;

    #endregion

    public static class RavenExtensions
    {
        #region Public Methods and Operators

        public static int Count<T>(this IRavenQueryable<T> results)
        {
            RavenQueryStatistics stats;
            results.Statistics(out stats).Take(0).ToList();
            return stats.TotalResults;
        }

        public static IEnumerable<T> ToList<T>(this IEnumerator<StreamResult<T>> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current.Document;
            }
        }

        public static T LoadFromNonPrefixedStringId<T>(this IDocumentSession session, string id)
        {
            return session.Load<T>(
                session.Advanced.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(
                    id,
                    typeof(T),
                    false));
        }

        #endregion
    }
}