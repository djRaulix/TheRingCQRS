namespace TheRing.CQRS.Eventing
{
    using System;

    public interface IEventSourcedRepository
    {
        #region Public Methods and Operators

        TEventSourced Create<TEventSourced>(Guid id)  where TEventSourced : EventSourced;

        TEventSourced Get<TEventSourced>(Guid id, int? expectedVersion = null)  where TEventSourced : EventSourced;

        void Save(EventSourced eventSourced);

        #endregion
    }
}