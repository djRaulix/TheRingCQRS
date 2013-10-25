namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Eventing;

    #endregion

    public class DenormalizerConsumer<TEvent> : Consumes<TEvent>.All
        where TEvent : Event
    {
        #region Fields

        private readonly IDenormalizeEvent<TEvent> denormalizeEvent;

        #endregion

        #region Constructors and Destructors

        public DenormalizerConsumer(IDenormalizeEvent<TEvent> denormalizeEvent)
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