namespace TheRing.CQRS.Eventing.EventSourced.Repository
{
    using System;

    public class UnKnownEventSourcedException : EventingException
    {
        public UnKnownEventSourcedException(Guid id) : base(string.Format("Unknown Event sourced id ({0})",id))
        {
            throw new NotImplementedException();
        }
    }
}