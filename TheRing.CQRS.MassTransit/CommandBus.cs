namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;

    using global::MassTransit;

    using Magnum.Extensions;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.Commanding.Handler;
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

        public RequestResult SendRequest<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            command.CorrelationId = correlationId;
            command.ExpectResponse = true;
            var response = RequestResult.Failed;
            this.requestEndPoint.SendRequest(
                command,
                this.serviceBus,
                c =>
                {
                    c.Handle<DoneResponse>(h => response = RequestResult.Ok);
                    c.Handle<ConcurrencyExceptionResponse>(h => response = RequestResult.ConcurrencyException);
                    c.HandleTimeout(30.Seconds(), h => response = RequestResult.Failed);
                    c.HandleFault(h => response = RequestResult.Failed);
                });
            return response;
        }

        #endregion
    }
}