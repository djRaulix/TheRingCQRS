namespace TheRing.CQRS.Domain
{
    #region using

    using System;
    using System.Collections.Generic;

    using TheRing.CQRS.Eventing;

    #endregion

    public abstract class AggregateRoot
    {
        #region Fields

        private readonly Queue<Event> changes = new Queue<Event>();

        private Guid currentCorrelationId;

        internal Guid Id {get; set;}

        internal int Version { get; private set; }

        #endregion

        #region Properties

        internal IEnumerable<Event> Changes
        {
            get { return this.changes; }
        }

        #endregion

        #region Public Methods and Operators

        public void SetCurrentCorrelationId(Guid correlationId)
        {
            this.currentCorrelationId = correlationId;
        }

        #endregion

        #region Methods

        internal void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var @event in history)
            {
                this.ApplyEvent(@event);
            }
        }

        private void ApplyEvent(Event @event)
        {
            this.Version++;
            if (!@event.Volatile)
            {
                this.ApplyGeneric(@event);
            }
        }

        protected virtual void ApplyGeneric(Event @event)
        {
        }

        protected void ApplyChange(Event @event)
        {
            this.ApplyEvent(@event);
            @event.EventSourcedId = this.Id;
            @event.EventSourcedVersion = this.Version;
            @event.TimeStamp = DateTime.UtcNow;
            @event.CorrelationId = this.currentCorrelationId;
            this.changes.Enqueue(@event);
        }

        #endregion
    }
}