namespace TheRing.CQRS.Eventing
{
    public interface IDenormalizeEvent<in TEvent>
        where TEvent : Event
    {
        #region Public Methods and Operators

        void Consume(TEvent @event);

        #endregion
    }
}