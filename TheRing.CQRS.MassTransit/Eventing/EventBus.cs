namespace TheRing.CQRS.MassTransit.Eventing
{
    using global::MassTransit;

    using TheRing.CQRS.Eventing;

    public class EventBus : IEventBus
    {
        #region Fields

        private readonly IServiceBus bus;

        #endregion

        #region Constructors and Destructors

        public EventBus(IServiceBus bus)
        {
            this.bus = bus;
        }

        #endregion

        #region Public Methods and Operators

        public void Publish(Event @event)
        {
            this.bus.Publish(@event);
        }

        #endregion
    }
}