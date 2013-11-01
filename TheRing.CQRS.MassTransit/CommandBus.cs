namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;
    using System.Linq;

    using global::MassTransit;

    using Magnum.Extensions;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.MassTransit.Properties;

    #endregion

    public class CommandBus : ICommandBus
    {
        #region Fields

        private readonly IEndpoint requestEndPoint;
        private readonly IServiceBus serviceBus;

        #endregion

        #region Constructors and Destructors

        public CommandBus(IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
            this.requestEndPoint = serviceBus.GetEndpoint(new Uri(Settings.Default.RequestQueue));
        }

        #endregion

        #region Public Methods and Operators

        public void Send<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            command.CorrelationId = correlationId;
            this.requestEndPoint.Send(command);
        }

        public Response SendRequest<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            var timeout = Settings.Default.CommandTimeOut;
            command.CorrelationId = correlationId;
            command.ExpectResponse = true;
            var response = new Response(true);
            this.requestEndPoint.SendRequest(
                command,
                this.serviceBus,
                c =>
                {
                    c.Handle<Response>(h => response = h);
                    c.HandleTimeout(
                        timeout.Seconds(),
                        h => response = new Response("TimeOut", string.Format("TimeOut after {0} seconds", timeout)));
                    c.HandleFault(h => response = new Response(h.FaultType, h.Messages.First()));
                });
            return response;
        }

        #endregion
    }
}