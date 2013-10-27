namespace TheRing.CQRS.Commanding.Handler
{
    public interface IHandleContext<out TCommand>
        where TCommand : AbstractCommand
    {
        #region Public Properties

        TCommand Command { get; }

        #endregion

        #region Public Methods and Operators

        void Respond<TResponse>(TResponse response) where TResponse : class;
        bool RetryLater();

        #endregion
    }
}