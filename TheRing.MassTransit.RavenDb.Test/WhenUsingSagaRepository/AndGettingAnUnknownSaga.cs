namespace TheRing.MassTransit.RavenDb.Test.WhenUsingSagaRepository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using global::MassTransit;
    using global::MassTransit.Saga;

    using NUnit.Framework;

    using TheRing.MassTransit.RavenDb.Test.Fakes;

    #endregion

    public class AndGettingAnUnknownSaga : WhenUsingSagaRepository
    {
        #region Fields

        private bool callBackCalled;
        private List<Action<IConsumeContext<FakeMessage>>> callBacks;
        private IConsumeContext<FakeMessage> consumeContext;
        private FakeSaga newSaga;
        private ISagaPolicy<FakeSaga, FakeMessage> policy;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenCallBackShouldBeExecuted()
        {
            this.callBackCalled.Should().BeTrue();
        }

        [Test]
        public void ThenNewSagaShouldBeCreated()
        {
            using (var session = this.SagaStore.OpenSession())
            {
                var storedSaga = session.Load<FakeSaga>(this.newSaga.CorrelationId);
                storedSaga.Should().NotBeNull();
            }
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();

            this.SagaRepository.GetSaga(
                this.consumeContext,
                this.newSaga.CorrelationId,
                (saga, message) => this.callBacks,
                this.policy).First()(this.consumeContext);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.newSaga = new FakeSaga
            {
                CorrelationId = Guid.NewGuid()
            };

            this.callBacks = new List<Action<IConsumeContext<FakeMessage>>> { context => this.callBackCalled = true };

            this.policy = A.Fake<ISagaPolicy<FakeSaga, FakeMessage>>();
            this.consumeContext = A.Fake<IConsumeContext<FakeMessage>>();
            this.policy.CallsTo(p => p.CanCreateInstance(this.consumeContext)).Returns(true);
            this.policy.CallsTo(p => p.CanRemoveInstance(this.newSaga)).Returns(false);
            this.policy.CallsTo(p => p.CreateInstance(this.consumeContext, this.newSaga.CorrelationId))
                .Returns(this.newSaga);
        }

        #endregion
    }
}