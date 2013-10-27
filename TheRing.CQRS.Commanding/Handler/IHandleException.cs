namespace TheRing.CQRS.Commanding.Handler
{
    using System;

    public interface IHandleException
    {
        bool MustRetry(Exception exception);
        object GetMessage(Exception exception);
    }
}