namespace TheRing.CQRS.Eventing.EventSourced
{
    #region using

    using System;
    using System.Collections.Generic;

    #endregion

    public abstract class AbstractEventSourced 
    {
        #region Fields

        private readonly Queue<AbstractEvent> changes = new Queue<AbstractEvent>();

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Properties

        public Guid Id { get; internal set; }

        #endregion

        #region Properties

        internal IEnumerable<AbstractEvent> Changes
        {
            get
            {
                return this.changes;
            }
        }

        internal int Version { get; private set; }

        protected Guid CurrentCorrelationId { private get; set; }

        #endregion

        #region Methods

        internal void LoadFromHistory(IEnumerable<AbstractEvent> history)
        {
            foreach (var @event in history)
            {
                this.Version = @event.EventSourcedVersion;
                this.ApplyEvent(@event);
            }
        }

        internal virtual void Restore(object backup)
        {
            var memento = backup as Memento;
            if (memento == null)
            {
                return;
            }

            memento.Restore(this);
        }

        internal virtual object Snapshot()
        {
            return new Memento(this);
        }

        protected void ApplyChange(AbstractEvent @event)
        {
            this.ApplyEvent(@event);
            this.Version++;
            @event.EventSourcedId = this.Id;
            @event.EventSourcedVersion = this.Version;
            @event.TimeStamp = DateTime.UtcNow;
            @event.CorrelationId = this.CurrentCorrelationId;
            this.changes.Enqueue(@event);
        }

        protected virtual object GetSnapshot()
        {
            throw new NotSupportedException();
        }

        protected virtual void RestoreFromSnapshot(object snapshot)
        {
            throw new NotSupportedException();
        }

        private void ApplyEvent(AbstractEvent @event)
        {
            if (!@event.Volatile)
            {
                this.ApplyGeneric(@event);
            }
        }

        protected virtual void ApplyGeneric(AbstractEvent @event)
        {
            throw new NotSupportedException();
        }

        #endregion

        private class Memento
        {
            #region Fields

            private readonly object payload;
            private readonly int version;

            #endregion

            #region Constructors and Destructors

            internal Memento(AbstractEventSourced eventSourced)
            {
                this.version = eventSourced.Version;
                this.payload = eventSourced.GetSnapshot();
            }

            #endregion

            #region Methods

            internal void Restore(AbstractEventSourced eventSourced)
            {
                eventSourced.Version = this.version;
                eventSourced.RestoreFromSnapshot(this.payload);
            }

            #endregion
        }
    }
}