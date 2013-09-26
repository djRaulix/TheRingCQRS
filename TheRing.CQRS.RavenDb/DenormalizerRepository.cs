namespace TheRing.CQRS.RavenDb
{
    using Raven.Client;

    public class DenormalizerRepository : AbstractDenormalizerRepository
    {
        public DenormalizerRepository(IDocumentStore documentStore)
            : base(documentStore)
        {
        }
    }
}