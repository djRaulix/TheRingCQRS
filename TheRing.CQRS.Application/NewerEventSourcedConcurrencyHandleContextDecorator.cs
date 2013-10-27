namespace TheRing.CQRS.Application
{
    #region using

    using System;

    using global::TheRing.CQRS.Commanding.Handler;
    using global::TheRing.CQRS.Eventing.EventSourced.Repository.Exception;

    #endregion

    public class NewerEventSourcedConcurrencyHandleExceptionDecorator : AbstractHandleExceptionDecorator
    {
        #region Constructors and Destructors

        public NewerEventSourcedConcurrencyHandleExceptionDecorator(IHandleException handler)
            : base(handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override object GetMessage(Exception exception)
        {
            if (exception is NewerEventSourcedVersionLoadedException)
            {
                return new ConcurrencyExceptionResponse();
            }
            return base.GetMessage(exception);
        }

        public override bool MustRetry(Exception exception)
        {
            return exception is NewerEventSourcedVersionLoadedException;
        }

        #endregion
    }
}