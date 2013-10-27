namespace TheRing.CQRS.Eventing.EventSourced.Factory
{
    public interface IEventSourcedFactory
    {
        T New<T>() where T : AbstractEventSourced;
    }
}