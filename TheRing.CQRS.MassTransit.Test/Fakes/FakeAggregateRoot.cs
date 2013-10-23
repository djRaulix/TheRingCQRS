namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    using System;

    using TheRing.CQRS.Domain;

    public class FakeAggregateRoot : IAggregateRoot
    {
        #region Implementation of IAggregateRoot

        public void SetCurrentCorrelationId(Guid correlationId)
        {
        }

        #endregion
    }
}