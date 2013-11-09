namespace TheRing.CQRS.Eventing
{
    using System;

    public class EventingException : Exception
    {
        public EventingException(string message) : base(message)
        {
            
        }
    }
}