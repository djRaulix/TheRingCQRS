namespace TheRing.CQRS.Eventing.RavenDb.Test.Fakes
{
    #region using

    using System.Collections.Generic;

    using TheRing.CQRS.Eventing;

    #endregion

    public class FakeEventBus : IEventBus
    {
        #region Constructors and Destructors

        public FakeEventBus()
        {
            this.PublishedEvents = new List<AbstractEvent>();
        }

        #endregion

        #region Public Properties

        public List<AbstractEvent> PublishedEvents { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Publish(AbstractEvent @event)
        {
            this.PublishedEvents.Add(@event);
        }

        #endregion
    }
}