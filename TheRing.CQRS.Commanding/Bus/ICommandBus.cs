namespace TheRing.CQRS.Commanding.Bus
{
    #region using

    using System;

    #endregion

    public interface ICommandBus
    {
        #region Public Methods and Operators

        void Send<T>(T command, Guid correlationId) where T : AbstractCommand;

        RequestResult SendRequest<T>(T command, Guid correlationId) where T : AbstractCommand;

        #endregion
    }
}