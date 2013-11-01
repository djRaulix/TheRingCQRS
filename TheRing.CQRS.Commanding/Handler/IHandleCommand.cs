namespace TheRing.CQRS.Commanding.Handler
{
    #region using

    using TheRing.CQRS.Commanding.Bus;

    #endregion

    public interface IHandleCommand<in TCommand>
        where TCommand : AbstractCommand
    {
        #region Public Methods and Operators

        Response Handle(TCommand command);

        #endregion
    }
}