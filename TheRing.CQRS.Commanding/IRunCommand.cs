namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

    public interface IRunCommand<in TAgg, in TCommand>
    {
        #region Public Methods and Operators

        void Run(TAgg aggregateRoot, TCommand command);

        #endregion
    }
}