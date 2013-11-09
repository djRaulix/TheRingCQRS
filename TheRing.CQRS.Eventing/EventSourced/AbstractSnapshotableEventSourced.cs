namespace TheRing.CQRS.Eventing.EventSourced
{
    using TheRing.CQRS.Eventing.EventSourced.Snapshot;

    public abstract class AbstractSnapshotableEventSourced : AbstractEventSourced, ISnaphotable
    {
        #region Public Methods and Operators

        public void Restore(object backup)
        {
            var memento = backup as Memento;
            if (memento == null)
            {
                return;
            }

            memento.Restore(this);
        }

        public object Snapshot()
        {
            var snapshot = this.GetSnapshot();
            return new Memento(snapshot, this.Version);
        }

        #endregion

        #region Methods

        protected abstract object GetSnapshot();

        protected abstract void RestoreFromSnapshot(object snapshot);

        #endregion

        private class Memento
        {
            #region Fields

            private readonly object payload;
            private readonly int version;

            #endregion

            #region Constructors and Destructors

            internal Memento(object snapshot, int version)
            {
                this.version = version;
                this.payload = snapshot;
            }

            #endregion

            #region Methods

            internal void Restore(AbstractSnapshotableEventSourced eventSourced)
            {
                eventSourced.Version = this.version;
                eventSourced.RestoreFromSnapshot(this.payload);
            }

            #endregion
        }
    }
}