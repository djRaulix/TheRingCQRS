namespace TheRing.CQRS.Commanding.Test.Fakes
{
    #region using

    using System;

    using TheRing.CQRS.Domain;

    #endregion

    public class FakeAggregateRoot : IAggregateRoot
    {
        #region Implementation of IAggregateRoot

        public void SetCurrentCorrelationId(Guid correlationId)
        {
        }

        #endregion
    }
}