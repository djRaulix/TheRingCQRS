namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

    public interface IEditAggregate<out TAgg>
        where TAgg : AggregateRoot
    {
        #region Public Methods and Operators

        TAgg Get(ICommand command);

        void Save(AggregateRoot aggregateRoot);

        #endregion
    }
}