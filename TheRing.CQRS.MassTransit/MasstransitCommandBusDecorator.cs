namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;
    using System.Linq;

    using global::MassTransit;

    using Magnum;
    using Magnum.Extensions;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.MassTransit.Properties;

    #endregion

    public class MassTransitCommandBusDecorator : AbstactCommandBusDecorator
    {
        #region Fields

        private readonly IEndpoint requestEndPoint;
        private readonly IServiceBus serviceBus;

        #endregion

        #region Constructors and Destructors

        public MassTransitCommandBusDecorator(IServiceBus serviceBus, ICommandBus commandBus): base(commandBus)
        {
            this.serviceBus = serviceBus;
            this.requestEndPoint = serviceBus.GetEndpoint(new Uri(Settings.Default.RequestQueue));
        }

        #endregion

        #region Public Methods and Operators


        public override void Send<T>(T command, Guid correlationId)
        {
            base.Send(command, correlationId);
            this.requestEndPoint.Send(command);
        }

        public override Response SendRequest<T>(T command, Guid correlationId)
        {
            var response = base.SendRequest(command, correlationId);
            var timeout = Settings.Default.CommandTimeOut;

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