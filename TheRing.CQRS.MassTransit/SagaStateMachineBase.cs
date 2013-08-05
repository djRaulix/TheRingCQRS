namespace TheRing.CQRS.MassTransit
{
    using System;

    using Magnum.StateMachine;

    using global::MassTransit;

    using global::MassTransit.Saga;

    using TheRing.CQRS.Commanding;

    public abstract class SagaStateMachineBase<T> : SagaStateMachine<T>, ISaga
        where T : SagaStateMachineBase<T>
    {
        #region Fields

        private IServiceBus bus;

        #endregion

        #region Constructors and Destructors

        protected SagaStateMachineBase(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }

        #endregion

        #region Public Properties

        public static State Completed { get; set; }

        public static State Initial { get; set; }

        public IServiceBus Bus
        {
            get
            {
                return this.bus;
            }

            set
            {
                this.bus = value;
                this.CommandBus = this.bus.CommandBus();
            }
        }

        public Guid CorrelationId { get; set; }

        #endregion

        #region Properties

        protected ICommandBus CommandBus { get; private set; }

        #endregion

        #region Methods

        protected static global::MassTransit.Saga.EventBinder<T, V> Correlate<V>(Event<V> targetEvent, Func<V, Guid> property)
        {
            return Correlate(targetEvent).By((s, e) => property(e) == s.CorrelationId).UseId(property);
        }

        #endregion
    }
}