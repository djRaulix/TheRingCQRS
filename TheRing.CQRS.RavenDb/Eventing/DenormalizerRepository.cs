namespace TheRing.CQRS.RavenDb.Eventing
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