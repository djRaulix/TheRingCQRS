namespace TheRing.CQRS.MassTransit
{
    using TheRing.CQRS.Commanding.Bus;

    public static class Services
    {
        public static ICommandBus CommandBus
        {
            internal get; set;
        }
    }
}