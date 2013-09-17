namespace TheRing.CQRS.Domain.UnitTests.Fakes
{
    using TheRing.CQRS.Eventing;

    public class FakeNonVolatileEvent : Event
    {
        #region Overrides of Event

        public override bool Volatile
        {
            get
            {
                return false;
            }
        }

        public string Property
        {
            get; set;
        }

        #endregion
    }
}