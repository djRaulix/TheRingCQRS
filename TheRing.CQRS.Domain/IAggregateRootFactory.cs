namespace TheRing.CQRS.Domain
{
    public interface IAggregateRootFactory
    {
        #region Public Methods and Operators

        T Create<T>() where T : AggregateRoot;

        #endregion
    }
}