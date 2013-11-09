namespace TheRing.CQRS.RavenDb.Eventing
{
    #region using

    using System.Linq;

    using Raven.Client.Indexes;

    using TheRing.CQRS.Eventing;

    #endregion

    public class AbstractEvent_EventSourcedIdAndVersion : AbstractIndexCreationTask<AbstractEvent>
    {
        #region Constructors and Destructors

        public AbstractEvent_EventSourcedIdAndVersion()
        {
            this.Map = events => from e in events
                orderby e.EventSourcedVersion
                select new { e.EventSourcedId, e.EventSourcedVersion };
        }

        #endregion
    }
}