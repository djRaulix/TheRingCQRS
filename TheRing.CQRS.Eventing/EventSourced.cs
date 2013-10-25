namespace TheRing.CQRS.Eventing
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    #endregion

    

    public abstract class EventSourced
    {
        protected EventSourced()
        {
            this.thisConcrete = this;
        }
        
        private class Memento
        {
            internal Memento(EventSourced eventSourced)
            {
                this.version = eventSourced.Version;
                this.payload = eventSourced.GetSnapshot();
            }

            internal void Restore(EventSourced eventSourced)
            {
                eventSourced.Version = this.version;
                eventSourced.RestoreFromSnapshot(this.payload);
            }
            private readonly int version;

            private readonly object payload;
        }
        
        protected virtual object GetSnapshot()
        {
            throw new NotImplementedException();
        }

        protected virtual void RestoreFromSnapshot(object snapshot)
        {
            throw new NotImplementedException();
        }
        
        internal virtual object Snapshot()
        {
            return new Memento(this);
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

        
        #region Fields

        private readonly Queue<Event> changes = new Queue<Event>();
        private readonly dynamic thisConcrete;

        protected Guid CurrentCorrelationId { private get; set; }

        public Guid Id { get; internal set; }

        internal int Version { get; private set; }

        #endregion

        #region Properties

        internal IEnumerable<Event> Changes
        {
            get { return this.changes; }
        }

        #endregion

        #region Public Methods and Operators

        #endregion

        #region Methods

        internal void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var @event in history)
            {
                this.Version = @event.EventSourcedVersion;
                this.ApplyEvent(@event);
            }
        }

        private void ApplyEvent(Event @event)
        {
            
            if (!@event.Volatile)
            {
                thisConcrete.Apply((dynamic)@event);
            }
        }

        protected void ApplyChange(Event @event)
        {
            this.ApplyEvent(@event);
            this.Version++;
            @event.EventSourcedId = this.Id;
            @event.EventSourcedVersion = this.Version;
            @event.TimeStamp = DateTime.UtcNow;
            @event.CorrelationId = this.CurrentCorrelationId;
            this.changes.Enqueue(@event);
        }

        #endregion
    }
}