namespace WebSample.Sagas
{
    #region using

    using System;

    using Magnum.StateMachine;

    using TheRing.CQRS.MassTransit;

    using WebSample.Commanding.User;
    using WebSample.Domain.User;

    #endregion

    public class CreateUserSaga : AbstractSagaStateMachineBase<CreateUserSaga>
    {
        #region Constructors and Destructors

        static CreateUserSaga()
        {
            Define(
                () =>
                {
                    Correlate(UserCreated).By((saga, message) => saga.CorrelationId == message.EventSourcedId);
                    Correlate(UserConfirmed).By((saga, message) => saga.CorrelationId == message.CorrelationId);
                    Initially(
                        When(UserCreated)
                            .Then(
                                (saga, message) =>
                                    saga.CommandBus.Send(
                                        new ConfirmUser { Id = saga.CorrelationId },
                                        saga.CorrelationId))
                            .TransitionTo(ConfirmingUser));
                    During(ConfirmingUser, When(UserConfirmed).Complete());
                });
        }

        public CreateUserSaga(Guid correlationId)
            : base(correlationId)
        {
        }

        #endregion

        #region Public Properties

        public static State ConfirmingUser { get; set; }

        public static Event<UserConfirmed> UserConfirmed { get; set; }

        public static Event<UserCreated> UserCreated { get; set; }

        #endregion
    }
}