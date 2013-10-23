namespace TheRing.CQRS.Eventing
{
    public interface IEventSourcedFactory
    {
        #region Public Methods and Operators

        T Create<T>() where T : EventSourced;

        #endregion
    }
}