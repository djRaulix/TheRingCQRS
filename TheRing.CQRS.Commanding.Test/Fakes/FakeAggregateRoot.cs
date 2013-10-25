namespace TheRing.CQRS.Commanding.Test.Fakes
{
    #region using

    using System;

    #endregion

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