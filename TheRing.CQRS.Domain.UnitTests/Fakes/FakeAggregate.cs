namespace TheRing.CQRS.Domain.UnitTests.Fakes
{
    #region using

    using TheRing.CQRS.Eventing;

    #endregion

    public class FakeAggregate : AggregateRoot
    {
        #region Public Properties

        public string NonVolatileProperty { get; set; }

        public string VolatileProperty { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Apply(FakeNonVolatileEvent @event)
        {
            this.NonVolatileProperty = @event.Property;
        }

        public void ApplyChangeInPublicMethod(Event @event)
        {
            this.ApplyChange(@event);
        }

        #endregion

        #region Methods

        protected override void ApplyGeneric(Event @event)
        {
            this.Apply((dynamic)@event);
        }

        #endregion
    }
}