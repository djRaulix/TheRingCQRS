namespace TheRing.CQRS.MassTransit.Commanding
{
    using System;

    using Magnum;
    using Magnum.Extensions;

    using global::MassTransit;

    using TheRing.CQRS.Commanding;

    public class CommandBus : ICommandBus
    {
        #region Fields

        private readonly IServiceBus bus;

        private Uri commandUri;

        #endregion

        #region Constructors and Destructors

        public CommandBus(IServiceBus bus)
        {
            this.bus = bus;
        }

        #endregion

        #region Public Methods and Operators

        public void Send<T>(T command, Guid correlationId) where T : class, ICommand, new()
        {
            command.SetCorrelationId(correlationId);
            this.CommandEndPoint().Send(command);
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

            var response = RequestResult.Ok;

            this.CommandEndPoint().SendRequest(
                command, 
                this.bus, 
                c =>
                    {
                        c.Handle<ConcurrencyExceptionResponse>(h => response = RequestResult.ConcurrencyException);
                        c.HandleTimeout(10.Seconds(), h => response = RequestResult.Failed);
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

        private IEndpoint CommandEndPoint()
        {
            return this.bus.GetEndpoint(this.commandUri);
        }

        #endregion
    }
}