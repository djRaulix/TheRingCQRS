namespace TheRing.CQRS.Eventing.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Eventing;

    #endregion

    public static class ServiceBusExtention
    {
        #region Public Methods and Operators

        public static IEventBus EventBus(this IServiceBus bus)
        {
            return new EventBus(bus);
        }

        #endregion
    }
}