namespace WebSample.Sagas
{
    #region using

    using System;

    using TheRing.CQRS.Commanding.MassTransit;

    #endregion

    public abstract class WebSampleSaga<TSaga> : SagaStateMachineBase<TSaga>
        where TSaga : SagaStateMachineBase<TSaga>
    {
        #region Constructors and Destructors

        protected WebSampleSaga(Guid correlationId)
            : base(correlationId)
        {
        }

        #endregion

        #region Properties

        protected override string RequestQueue
        {
            get
            {
                return Constants.RequestQueue;
            }
        }

        #endregion
    }
}