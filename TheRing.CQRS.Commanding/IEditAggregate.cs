namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

    public interface IEditAggregate<TAgg> where TAgg : IAggregateRoot
    {
        #region Public Methods and Operators

        TAgg Get(ICommand command);

        void Save(TAgg aggregateRoot);

        #endregion
    }
}