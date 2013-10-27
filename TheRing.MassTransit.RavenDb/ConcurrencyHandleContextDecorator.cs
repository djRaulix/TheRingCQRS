namespace TheRing.CQRS.MassTransit.RavenDb
{
    #region using

    using System;

    using Raven.Abstractions.Exceptions;

    using TheRing.CQRS.Commanding.Handler;

    #endregion

    public class ConcurrencyHandleExceptionDecorator : AbstractHandleExceptionDecorator
    {
        #region Constructors and Destructors

        public ConcurrencyHandleExceptionDecorator(IHandleException handler)
            : base(handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override object GetMessage(Exception exception)
        {
            if ((exception is ConcurrencyException))
            {
                return new ConcurrencyExceptionResponse();
            }
            return base.GetMessage(exception);
        }

        public override bool MustRetry(Exception exception)
        {
            return (exception is ConcurrencyException);
        }

        #endregion
    }
}