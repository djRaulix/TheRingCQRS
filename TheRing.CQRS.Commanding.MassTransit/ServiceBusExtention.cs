namespace TheRing.CQRS.Commanding.MassTransit
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;

    #endregion

    public static class ServiceBusExtention
    {
        #region Public Methods and Operators

        public static ICommandBus CommandBus(this IServiceBus bus, string requestQueue)
        {
            return new CommandBus(bus, requestQueue);
        }

        #endregion
    }
}