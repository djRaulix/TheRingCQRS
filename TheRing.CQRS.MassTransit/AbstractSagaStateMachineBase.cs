namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;

    using global::MassTransit;
    using global::MassTransit.Saga;

    using Magnum.StateMachine;

    using TheRing.CQRS.Commanding.Bus;

    #endregion

    public abstract class AbstractSagaStateMachineBase<T> : SagaStateMachine<T>, ISaga
        where T : AbstractSagaStateMachineBase<T>
    {
        #region Constructors and Destructors

        protected AbstractSagaStateMachineBase(Guid correlationId)
        {
            this.CorrelationId = correlationId;
            this.CommandBus = Services.CommandBus;
        }

        #endregion

        #region Public Properties

        protected ICommandBus CommandBus { get; set; }

        public static State Completed { get; set; }

        public static State Initial { get; set; }

        public IServiceBus Bus { get; set; }

        public Guid CorrelationId { get; set; }

        #endregion

        #region Methods

        protected static global::MassTransit.Saga.EventBinder<T, V> Correlate<V>(
            Event<V> targetEvent,
            Func<V, Guid> property)
        {
            return Correlate(targetEvent).By((s, e) => property(e) == s.CorrelationId).UseId(property);
        }

        #endregion
    }
}