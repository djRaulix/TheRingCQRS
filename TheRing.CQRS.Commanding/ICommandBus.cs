namespace TheRing.CQRS.Commanding
{
    using System;

    public interface ICommandBus
    {
        #region Public Methods and Operators

        void Send<T>(T command) where T : class, ICommand, new();

        void Send<T>(T command, Guid correlationId) where T : class, ICommand, new();

        bool SendOk<T>(T command) where T : class, ICommand, new();

        bool SendOk<T>(T command, Guid correlationId) where T : class, ICommand, new();

        RequestResult SendRequest<T>(T command) where T : class, ICommand, new();

        RequestResult SendRequest<T>(T command, Guid correlationId) where T : class, ICommand, new();

        #endregion
    }
}