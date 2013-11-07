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

        private dynamic concrete;

        #endregion

        #region Constructors and Destructors

        protected AbstractEventSourced()
        {
            this.concrete = this;
        }

        #endregion

        #region Public Properties

        public Guid Id { get; internal set; }

        public int Version { get; protected internal set; }

        #endregion

        #region Properties

        internal IEnumerable<AbstractEvent> Changes
        {
            get
            {
                return this.changes;
            }
        }

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
          
        internal void Apply(AbstractEvent @event)
        {
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

        private void ApplyEvent(dynamic @event)
        {
            concrete.Apply(@event);
        }

        #endregion
    }
}