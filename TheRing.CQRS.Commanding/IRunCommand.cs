namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

    public interface IRunCommand<in TAgg, in TCommand>
        where TAgg : AggregateRoot where TCommand : ICommand
    {
        #region Public Methods and Operators

        void Run(TAgg aggregateRoot, TCommand command);

        #endregion
    }
}