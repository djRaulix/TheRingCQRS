namespace TheRing.CQRS.Commanding
{
    using System;

    [Serializable]
    public class AggregateRootConcurrencyException : Exception
    {
        #region Constructors and Destructors

        public AggregateRootConcurrencyException(string message)
            : base(message)
        {
        }

        #endregion
    }
}