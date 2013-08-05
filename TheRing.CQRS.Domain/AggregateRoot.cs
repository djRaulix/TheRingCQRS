namespace TheRing.CQRS.Domain
{
    using System;
    using System.Collections.Generic;

    using TheRing.CQRS.Eventing;

    public abstract class AggregateRoot
    {
        #region Fields

        private readonly Queue<Event> changes = new Queue<Event>();

        private Guid currentCorrelationId;

        private Guid id;

        private int version;

        #endregion

        #region Properties

        internal IEnumerable<Event> Changes
        {
            get
            {
                return this.changes;
            }
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

        internal void SetId(Guid aggId)
        {
            this.id = aggId;
        }

        protected void ApplyChange(Event @event)
        {
            this.ApplyEvent(@event);
            @event.EventSourcedId = this.id;
            @event.EventSourcedVersion = this.version;
            @event.TimeStamp = DateTime.UtcNow;
            @event.CorrelationId = this.currentCorrelationId;
            this.changes.Enqueue(@event);
        }

        protected virtual void ApplyGeneric(Event @event)
        {
        }

        private void ApplyEvent(Event @event)
        {
            this.version++;
            if (!@event.Volatile)
            {
                this.ApplyGeneric(@event);
            }
        }

        #endregion
    }
}