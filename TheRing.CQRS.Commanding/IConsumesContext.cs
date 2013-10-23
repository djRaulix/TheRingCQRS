namespace TheRing.CQRS.Commanding
{
    public interface IConsumesContext<out TCommand>
        where TCommand : class, ICommand
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