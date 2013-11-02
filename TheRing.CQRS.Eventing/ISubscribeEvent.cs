namespace TheRing.CQRS.Eventing
{
    public interface ISubscribeEvent<in TEvent>
        where TEvent : AbstractEvent
    {
        #region Public Methods and Operators

        void Consume(TEvent @event);

        #endregion
    }
}