namespace TheRing.CQRS.Commanding
{
    using TheRing.CQRS.Domain;

    public interface IEditAggregate<out TAgg>
        where TAgg : AggregateRoot
    {
        #region Public Methods and Operators

        TAgg Get(ICommand command);

        void Save(AggregateRoot aggregateRoot);

        #endregion
    }
}