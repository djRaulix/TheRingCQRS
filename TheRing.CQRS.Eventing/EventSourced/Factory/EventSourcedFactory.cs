namespace TheRing.CQRS.Eventing.EventSourced.Factory
{
    using System;

    public class EventSourcedFactory : IEventSourcedFactory
    {
        private readonly Func<Type, object> factory;

        public EventSourcedFactory(Func<Type,object> factory)
        {
            this.factory = factory;
        }

        #region Implementation of IEventSourcedFactory

        public T New<T>() where T : AbstractEventSourced
        {
            return (T)this.factory(typeof(T));
        }

        #endregion
    }
}