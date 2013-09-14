namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

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
            var updateCommand = command as UpdateCommand;
            return updateCommand == null
                ? this.repository.Create<TAgg>(command.Id)
                : this.repository.Get<TAgg>(command.Id, updateCommand.ExpectedVersion);
        }

        public void Save(AggregateRoot aggregateRoot)
        {
            this.repository.Save(aggregateRoot);
        }

        #endregion
    }
}