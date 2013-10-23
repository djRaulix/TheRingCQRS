namespace TheRing.CQRS.Eventing
{
    using System;

    public interface IEventSourcedRepository<out TEventSourced> where TEventSourced : EventSourced
    {
        #region Public Methods and Operators

        TEventSourced Create(Guid id);

        TEventSourced Get(Guid id, int? expectedVersion = null);

        void Save(EventSourced eventSourced);

        #endregion
    }
}