namespace TheRing.CQRS.MassTransit.Querying
{
    using global::MassTransit;

    using TheRing.CQRS.Eventing;

    public class Denormalizer<TEvent> : Consumes<TEvent>.All
        where TEvent : Event
    {
        #region Fields

        private readonly IDenormalizeEvent<TEvent> denormalizeEvent;

        #endregion

        #region Constructors and Destructors

        public Denormalizer(IDenormalizeEvent<TEvent> denormalizeEvent)
        {
            this.denormalizeEvent = denormalizeEvent;
        }

        #endregion

        #region Public Methods and Operators

        public void Consume(TEvent message)
        {
            this.denormalizeEvent.Consume(message);
        }

        #endregion
    }
}