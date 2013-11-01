namespace TheRing.CQRS.Eventing.EventSourced.Snapshot
{
    #region using

    using System;
    using System.Collections.Concurrent;

    #endregion

    public class InMemorySnapshotKeeper : IKeepSnapshot
    {
        #region Fields

        private readonly ConcurrentDictionary<Guid, object> memory =
            new ConcurrentDictionary<Guid, object>();

        #endregion

        #region Public Methods and Operators

        public void Delete(ISnaphotable snaphotable)
        {
            object snapshot;
            this.memory.TryRemove(snaphotable.Id, out snapshot);
        }

        public void Restore(ISnaphotable eventSourced)
        {
            object snapshot;

            this.memory.TryGetValue(eventSourced.Id, out snapshot);
            eventSourced.Restore(snapshot);
        }

        public void Snapshot(ISnaphotable eventSourced)
        {
            this.memory[eventSourced.Id] = eventSourced.Snapshot();
        }

        #endregion
    }
}