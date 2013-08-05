namespace TheRing.CQRS.MassTransit
{
    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.MassTransit.Commanding;
    using TheRing.CQRS.MassTransit.Eventing;

    public static class ServiceBusExtention
    {
        #region Public Methods and Operators

        public static ICommandBus CommandBus(this IServiceBus bus)
        {
            return new CommandBus(bus);
        }

        public static IEventBus EventBus(this IServiceBus bus)
        {
            return new EventBus(bus);
        }

        #endregion
    }
}