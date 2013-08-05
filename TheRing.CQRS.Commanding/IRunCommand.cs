namespace TheRing.CQRS.Commanding
{
    using TheRing.CQRS.Domain;

    public interface IRunCommand<in TAgg, in TCommand>
        where TAgg : AggregateRoot where TCommand : ICommand
    {
        #region Public Methods and Operators

        void Run(TAgg aggregateRoot, TCommand command);

        #endregion
    }
}