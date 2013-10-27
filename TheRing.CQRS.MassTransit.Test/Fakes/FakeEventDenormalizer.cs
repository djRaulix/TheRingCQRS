namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    using TheRing.CQRS.Eventing;

    public class FakeEventDenormalizer : ISubscribesEvent<FakeEvent>
    {
        #region Implementation of IDenormalizeEvent<in FakeEvent>

        public FakeEvent LastConsumedEvent
        {
            get;
            private set;
        }

        public void Consume(FakeEvent @event)
        {
            this.LastConsumedEvent = @event;
        }

        #endregion
    }
}