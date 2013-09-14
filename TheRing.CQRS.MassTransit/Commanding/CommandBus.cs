namespace TheRing.CQRS.MassTransit.Commanding
{
    #region using

    using System;

    using Magnum;

    using global::MassTransit;

    using TheRing.CQRS.Commanding;

    #endregion

    public class CommandBus : ICommandBus
    {
        #region Fields

        private readonly IServiceBus bus;

        private Uri commandUri;

        #endregion

        #region Constructors and Destructors

        public CommandBus(IServiceBus bus, string requestQueue)
        {
            this.bus = bus;
            this.commandUri = new Uri(requestQueue);
        }

        #endregion

        #region Public Methods and Operators

        public void Send<T>(T command, Guid correlationId) where T : class, ICommand, new()
        {
            command.SetCorrelationId(correlationId);
            this.RequestEndPoint().Send(command);
        }

        public void Send<T>(T command) where T : class, ICommand, new()
        {
            this.Send(command, CombGuid.Generate());
        }

        public bool SendOk<T>(T command, Guid correlationId) where T : class, ICommand, new()
        {
            return this.SendRequest(command, correlationId) == RequestResult.Ok;
        }

        public bool SendOk<T>(T command) where T : class, ICommand, new()
        {
            return this.SendRequest(command) == RequestResult.Ok;
        }

        public RequestResult SendRequest<T>(T command, Guid correlationId) where T : class, ICommand, new()
        {
            command.SetCorrelationId(correlationId);
            command.ExpectResponse = true;
            var response = RequestResult.Failed;

            this.RequestEndPoint().SendRequest(
                command, 
                this.bus, 
                c =>
                {
                    c.Handle<DoneResponse>(h => response = RequestResult.Ok);
                    c.Handle<ConcurrencyExceptionResponse>(h => response = RequestResult.ConcurrencyException);


// c.HandleTimeout(10.Seconds(), h => response = RequestResult.Failed);
                    c.HandleFault(h => response = RequestResult.Failed);
                });
            return response;
        }

        public RequestResult SendRequest<T>(T command) where T : class, ICommand, new()
        {
            return this.SendRequest(command, CombGuid.Generate());
        }

        public void SetRequestQueue(string queue)
        {
            this.commandUri = new Uri(queue);
        }

        #endregion

        #region Methods

        private IEndpoint RequestEndPoint()
        {
            return this.bus.GetEndpoint(this.commandUri);
        }

        #endregion
    }
}