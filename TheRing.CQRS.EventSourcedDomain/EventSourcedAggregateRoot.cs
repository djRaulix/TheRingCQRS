namespace TheRing.CQRS.EventSourcedDomain
{
    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;

    public abstract class EventSourcedAggregateRoot : EventSourced, IAggregateRoot
    {
    }
}