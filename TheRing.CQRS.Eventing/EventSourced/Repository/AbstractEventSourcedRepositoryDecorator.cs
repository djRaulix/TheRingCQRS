namespace TheRing.CQRS.Eventing.EventSourced.Repository
{
    #region using

    using System;

    #endregion

    public abstract class AbstractEventSourcedRepositoryDecorator<TEventSourced> : IEventSourcedRepository<TEventSourced>
        where TEventSourced : AbstractEventSourced
    {
        #region Fields

        private readonly IEventSourcedRepository<TEventSourced> repository;

        #endregion

        #region Constructors and Destructors

        protected AbstractEventSourcedRepositoryDecorator(IEventSourcedRepository<TEventSourced> repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Public Methods and Operators

        public virtual TEventSourced Create(Guid id)
        {
            return this.repository.Create(id);
        }

        public virtual TEventSourced Get(Guid id, int? expectedVersion = null)
        {
            return this.repository.Get(id,expectedVersion);
        }

        public virtual void Refresh(AbstractEventSourced eventSourced)
        {
            this.repository.Refresh(eventSourced);
        }

        public virtual void Save(AbstractEventSourced eventSourced)
        {
            this.repository.Save(eventSourced);
        }

        #endregion
    }
}