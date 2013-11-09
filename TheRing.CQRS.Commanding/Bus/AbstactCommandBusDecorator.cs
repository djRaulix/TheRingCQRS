namespace TheRing.CQRS.Commanding.Bus
{
    using System;

    public abstract class AbstactCommandBusDecorator : ICommandBus
    {
        private readonly ICommandBus bus;

        protected AbstactCommandBusDecorator(ICommandBus bus)
        {
            this.bus = bus;
        }

        #region Implementation of ICommandBus

        public virtual void Send<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            this.bus.Send(command,correlationId);
        }

        public virtual Response SendRequest<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            return this.bus.SendRequest(command,correlationId);
        }

        #endregion
    }
}