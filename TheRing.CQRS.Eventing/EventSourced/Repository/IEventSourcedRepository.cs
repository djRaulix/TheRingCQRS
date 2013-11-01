namespace TheRing.CQRS.Eventing.EventSourced.Repository
{
    #region using

    using System;

    #endregion

    public interface IEventSourcedRepository
    {
        #region Public Methods and Operators

        TEventSourced Create<TEventSourced>(Guid id) where TEventSourced : AbstractEventSourced;
        TEventSourced Get<TEventSourced>(Guid id) where TEventSourced : AbstractEventSourced;

        void Save(AbstractEventSourced eventSourced);

        #endregion
    }
}