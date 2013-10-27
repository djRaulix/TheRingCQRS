namespace TheRing.CQRS.Eventing.EventSourced.Snapshot
{
    using System;
    using System.Collections.Concurrent;

    public class InMemorySnapshotKeeper : ISnapshotKeeper
    {
        #region Implementation of ISnapshotKeeper

        private readonly ConcurrentDictionary<Guid, object> memory = new ConcurrentDictionary<Guid, object>();

        public void Delete(Guid id)
        {
            object snapshot;
            this.memory.TryRemove(id, out snapshot);
        }

        public object Get(Guid id)
        {
            object snapshot;
            this.memory.TryGetValue(id, out snapshot);
            return snapshot;
        }

        public void Set(Guid id, object snapshot)
        {
            this.memory[id] = snapshot;
        }

        #endregion
    }
}