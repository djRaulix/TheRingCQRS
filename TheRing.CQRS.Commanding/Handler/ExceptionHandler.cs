namespace TheRing.CQRS.Commanding.Handler
{
    #region using

    using System;

    #endregion

    public class ExceptionHandler : IHandleException
    {
        #region Public Methods and Operators

        public object GetMessage(Exception exception)
        {
            throw new Exception("unhandled exception", exception);
        }

        public bool MustRetry(Exception exception)
        {
            return false;
        }

        #endregion
    }
}