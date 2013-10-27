namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.EventSourced;

    #endregion

    public class EventConsumer<TEvent> : Consumes<TEvent>.All
        where TEvent : AbstractEvent
    {
        #region Fields

        private readonly ISubscribeEvent<TEvent> subscriber;

        #endregion

        #region Constructors and Destructors

        public EventConsumer(ISubscribeEvent<TEvent> subscriber)
        {
            this.subscriber = subscriber;
        }

        #endregion

        #region Public Methods and Operators

        public void Consume(TEvent message)
        {
            this.subscriber.Consume(message);
        }

        #endregion
    }
}