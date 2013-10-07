namespace TheRing.CQRS.MassTransit.Test.WhenUsingCommandBus
{
    #region using

    using System;

    using FluentAssertions;

    using global::MassTransit.Context;

    using NUnit.Framework;

    using TheRing.CQRS.MassTransit.Test.Fakes;
    using TheRing.CQRS.RavenDb.Test;

    #endregion

    public class AndSendingCommand : WhenUsingCommandBus
    {
        #region Fields

        private FakeCommand command;

        #endregion

        #region Public Methods and Operators

        [Test]
        [Ignore("can't get it work with loopback")]
        public void ThenCommandHandlerShouldReceiveTheCommand()
        {
            GlobalSpecification.FakeCommandHandler.LastFakeCommand.Id.Should()
                .Be(this.command.Id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.CommandBus.Send(this.command);

            GlobalSpecification.CommandRequestServiceBus.Endpoint.InboundTransport.Receive(c1 => c2 => { }, 1000.Milliseconds());
            
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.command = new FakeCommand { Id = Guid.NewGuid() };
        }

        #endregion
    }
}