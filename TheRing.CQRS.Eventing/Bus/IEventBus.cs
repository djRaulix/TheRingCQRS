namespace TheRing.CQRS.Eventing.Bus
{
    public interface IEventBus
    {
        #region Public Methods and Operators

        void Publish(AbstractEvent @event);

        #endregion
    }
}