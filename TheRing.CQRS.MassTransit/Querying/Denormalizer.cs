namespace TheRing.CQRS.MassTransit.Querying
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Eventing;

    #endregion

    public class Denormalizer<TDenormalizer, TEvent> : Consumes<TEvent>.All
        where TDenormalizer : IDenormalizeEvent<TEvent> where TEvent : Event
    {
        #region Fields

        private readonly TDenormalizer denormalizeEvent;

        #endregion

        #region Constructors and Destructors

        public Denormalizer(TDenormalizer denormalizeEvent)
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