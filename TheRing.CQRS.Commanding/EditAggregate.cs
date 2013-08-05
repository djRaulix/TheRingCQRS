namespace TheRing.CQRS.Commanding
{
    using TheRing.CQRS.Domain;

    public class EditAggregate<TAgg> : IEditAggregate<TAgg>
        where TAgg : AggregateRoot
    {
        #region Fields

        private readonly IRepository repository;

        #endregion

        #region Constructors and Destructors

        public EditAggregate(IRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Get(ICommand command)
        {
            var updateCommand = command as IUpdateCommand;
            return updateCommand == null ? this.repository.Create<TAgg>() : this.repository.Get<TAgg>(updateCommand.Id, updateCommand.ExpectedVersion);
        }

        public void Save(AggregateRoot aggregateRoot)
        {
            this.repository.Save(aggregateRoot);
        }

        #endregion
    }
}