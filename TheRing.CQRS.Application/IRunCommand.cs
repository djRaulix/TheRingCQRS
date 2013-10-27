namespace TheRing.CQRS.Application
{
    #region using

    using global::TheRing.CQRS.Eventing.EventSourced;

    #endregion

    public interface IRunCommand<in TEventSourced, in TCommand> where TEventSourced : AbstractEventSourced
        where TCommand : AbstractEditCommand
    {
        #region Public Methods and Operators

        void Run(TEventSourced eventSourced, TCommand command);

        #endregion
    }
}