namespace TheRing.CQRS.Eventing
{
    using System;

    public class EventSourcedFactory : IEventSourcedFactory
    {
        #region Fields

        private readonly Func<Type, object> objectFactory;

        #endregion

        #region Constructors and Destructors

        public EventSourcedFactory(Func<Type, object> objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        #endregion

        #region Public Methods and Operators

        public T Create<T>() where T : EventSourced
        {
            return (T)this.objectFactory(typeof(T));
        }

        #endregion
    }
}