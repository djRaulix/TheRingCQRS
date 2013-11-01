namespace TheRing.CQRS.Commanding.Bus
{
    using System;

    using Magnum;

    public static class CommandBusExtensions
    {
        public static void Send<T>(this ICommandBus bus,T command) where T : AbstractCommand, new()
        {
            bus.Send(command, CombGuid.Generate());
        }

        public static Response SendRequest<T>(this ICommandBus bus, T command) where T : AbstractCommand, new()
        {
            return bus.SendRequest(command, CombGuid.Generate());
        }
    }
}