namespace TheRing.CQRS.Eventing.EventSourced.Repository
{
    #region using

    using System;

    #endregion

    public interface IEventSourcedRepository<out TEventSourced> where TEventSourced : AbstractEventSourced
    {
        #region Public Methods and Operators

        TEventSourced Create(Guid id);
        TEventSourced Get(Guid id, int? expectedVersion = null);
        void Refresh(AbstractEventSourced eventSourced);

        void Save(AbstractEventSourced eventSourced);

        #endregion
    }
}