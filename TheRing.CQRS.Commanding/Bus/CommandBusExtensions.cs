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

        public static bool SendOk<T>(this ICommandBus bus, T command, Guid correlationId) where T : AbstractCommand, new()
        {
            return bus.SendRequest(command, correlationId) == RequestResult.Ok;
        }

        public static bool SendOk<T>(this ICommandBus bus, T command) where T : AbstractCommand, new()
        {
            return bus.SendRequest(command) == RequestResult.Ok;
        }

        public static RequestResult SendRequest<T>(this ICommandBus bus, T command) where T : AbstractCommand, new()
        {
            return bus.SendRequest(command, CombGuid.Generate());
        }
    }
}