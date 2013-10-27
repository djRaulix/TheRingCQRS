namespace TheRing.CQRS.Commanding.Handler
{
    public interface IRunCommand<in TCommand>
        where TCommand : AbstractCommand
    {
        #region Public Methods and Operators

        void Run(TCommand command);

        #endregion
    }
}