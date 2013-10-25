namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Eventing;

    #endregion

    public static class ServiceBusExtention
    {
        #region Public Methods and Operators

        public static IEventBus EventBus(this IServiceBus bus)
        {
            return new EventBus(bus);
        }

        public static ICommandBus CommandBus(this IServiceBus bus, string requestQueue)
        {
            return new CommandBus(bus, requestQueue);
        }

        #endregion
    }
}