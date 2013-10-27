namespace TheRing.CQRS.Commanding.Handler
{
    #region using

    using System;

    #endregion

    public abstract class AbstractHandleExceptionDecorator : IHandleException
    {
        #region Fields

        private readonly IHandleException handler;

        #endregion

        #region Constructors and Destructors

        protected AbstractHandleExceptionDecorator(IHandleException handler)
        {
            this.handler = handler;
        }

        #endregion

        #region Public Methods and Operators

        public virtual object GetMessage(Exception exception)
        {
            return this.handler.GetMessage(exception);
        }

        public virtual bool MustRetry(Exception exception)
        {
            return this.handler.MustRetry(exception);
        }

        #endregion
    }
}