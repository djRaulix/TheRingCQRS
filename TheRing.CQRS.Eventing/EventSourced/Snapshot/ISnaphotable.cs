namespace TheRing.CQRS.Eventing.EventSourced.Snapshot
{
    using System;

    public interface ISnaphotable
    {
        void Restore(object backup);

        object Snapshot();

        Guid Id { get;}
    }
}