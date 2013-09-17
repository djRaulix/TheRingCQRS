namespace TheRing.CQRS.Domain.UnitTests.Fakes
{
    using TheRing.CQRS.Eventing;

    public class FakeVolatileEvent : Event
    {
        #region Overrides of Event

        public override bool Volatile
        {
            get
            {
                return true;
            }
        }

        public string Property
        {
            get;
            set;
        }

        #endregion
    }
}