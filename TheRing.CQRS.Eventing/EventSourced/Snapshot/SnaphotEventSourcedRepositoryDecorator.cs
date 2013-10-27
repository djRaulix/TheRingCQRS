namespace TheRing.CQRS.Eventing.EventSourced.Snapshot
{
    


    #region using

    using TheRing.CQRS.Eventing.EventSourced.Repository;

    #endregion

    public class SnaphotEventSourcedRepositoryDecorator<TEventSourced> :
        AbstractEventSourcedRepositoryDecorator<TEventSourced> where TEventSourced : AbstractEventSourced
    {
        #region Fields

        private readonly ISnapshotKeeper keeper;

        #endregion

        #region Constructors and Destructors

        public SnaphotEventSourcedRepositoryDecorator(
            IEventSourcedRepository<TEventSourced> repository,
            ISnapshotKeeper keeper)
            : base(repository)
        {
            this.keeper = keeper;
        }

        #endregion

        #region Public Methods and Operators

        public override void Refresh(AbstractEventSourced eventSourced)
        {
            if (eventSourced.Version != 0) return;
            try
            {
                eventSourced.Restore(this.keeper.Get(eventSourced.Id));
            }
            catch
            {
                this.keeper.Delete(eventSourced.Id);
                eventSourced.Restore(this.Create(eventSourced.Id).Snapshot());
            }
        }

        public override void Save(AbstractEventSourced eventSourced)
        {
            base.Save(eventSourced);
            try
            {
                this.keeper.Set(eventSourced.Id, eventSourced.Snapshot());
            }
            catch
            {
            }
        }

        #endregion
    }
}