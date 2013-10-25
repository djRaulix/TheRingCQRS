namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    using System;

    using TheRing.CQRS.Commanding;

    public class FakeAggregateRoot : IAggregateRoot
    {
        #region Implementation of IAggregateRoot

        public void SetCurrentCorrelationId(Guid correlationId)
        {
        }

        public void RunGeneric(ICommand command)
        {
        }

        #endregion
    }
}