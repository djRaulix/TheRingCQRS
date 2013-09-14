namespace TheRing.CQRS.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.MassTransit.Commanding;
    using TheRing.CQRS.MassTransit.Eventing;

    #endregion

    public static class ServiceBusExtention
    {
        #region Public Methods and Operators

        public static ICommandBus CommandBus(this IServiceBus bus, string requestQueue)
        {
            return new CommandBus(bus, requestQueue);
        }

        public static IEventBus EventBus(this IServiceBus bus)
        {
            return new EventBus(bus);
        }

        #endregion
    }
}