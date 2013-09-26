namespace TheRing.CQRS.RavenDb
{
    #region using

    using System;
    using System.Linq;

    using Raven.Client.Indexes;

    using TheRing.CQRS.Eventing;

    #endregion

    public class Event_EventSourcedIdAndVersion : AbstractIndexCreationTask<Event>
    {      
        #region Constructors and Destructors

        public Event_EventSourcedIdAndVersion()
        {
            this.Map = events => from e in events
                select new { e.EventSourcedId , e.EventSourcedVersion};
        }

        #endregion
    }
}