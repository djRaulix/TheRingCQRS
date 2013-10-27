namespace TheRing.CQRS.Eventing.Bus
{
    using System.Collections.Generic;

    public static class EventBusExtensions
    {
        public static void Publish(this IEventBus bus,IEnumerable<AbstractEvent> events)
        {
            foreach (var @event in events)
            {
                bus.Publish(@event);
            }
        }
    }
}