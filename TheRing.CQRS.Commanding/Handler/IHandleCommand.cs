namespace TheRing.CQRS.Commanding.Handler
{
    #region using

    

    #endregion

    public interface IHandleCommand<in TCommand>
        where TCommand : AbstractCommand
    {
        #region Public Methods and Operators

        void Handle(IHandleContext<TCommand> handleContext);

        #endregion
    }
}