namespace TheRing.CQRS.MassTransit.Test.WhenUsingCommandBus
{
    #region using

    using System;

    using FluentAssertions;

    using global::MassTransit.Context;

    using NUnit.Framework;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.MassTransit.Test.Fakes;
    using TheRing.CQRS.RavenDb.Test;

    #endregion

    public class AndSendingAValidCommand : WhenUsingCommandBus
    {
        #region Fields

        private FakeCommand command;
        private RequestResult result;

        #endregion

        #region Public Methods and Operators


        [Test]
        [Ignore("can't get it work with loopback")]
        public void ThenDoneResonseShouldBeReturned()
        {
            this.result.Should().Be(RequestResult.Ok);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.result = this.CommandBus.SendRequest(this.command);

            GlobalSpecification.CommandRequestServiceBus.Endpoint.InboundTransport.Receive(c1 => c2 => { }, 1000.Milliseconds());
            GlobalSpecification.CommandResponseServiceBus.Endpoint.InboundTransport.Receive(c1 => c2 => { }, 1000.Milliseconds());
            
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.command = new FakeCommand { Id = Guid.NewGuid() };
        }

        #endregion
    }
}