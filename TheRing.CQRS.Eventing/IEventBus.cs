namespace TheRing.CQRS.Eventing
{
    public interface IEventBus
    {
        #region Public Methods and Operators

        void Publish(Event @event);

        #endregion
    }
}