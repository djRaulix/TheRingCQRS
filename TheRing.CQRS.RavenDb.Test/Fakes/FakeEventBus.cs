namespace TheRing.CQRS.RavenDb.Test.Fakes
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
            this.PublishedEvents = new List<Event>();
        }

        #endregion

        #region Public Properties

        public List<Event> PublishedEvents { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Publish(Event @event)
        {
            this.PublishedEvents.Add(@event);
        }

        #endregion
    }
}