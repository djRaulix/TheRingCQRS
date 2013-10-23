namespace TheRing.CQRS.Domain
{
    using System;

    public interface IAggregateRoot
    {
        void SetCurrentCorrelationId(Guid correlationId);
    }
}