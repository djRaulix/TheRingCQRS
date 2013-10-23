namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

    public class EditAggregate<TAgg> : IEditAggregate<TAgg> where TAgg : IAggregateRoot
    {
        #region Fields

        private readonly IAggregateRootRepository<TAgg> repository;

        #endregion

        #region Constructors and Destructors

        public EditAggregate(IAggregateRootRepository<TAgg> repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Public Methods and Operators

        public TAgg Get(ICommand command)
        {
            var updateCommand = command as UpdateCommand;
            return updateCommand == null
                ? this.repository.Create(command.Id)
                : this.repository.Get(command.Id, updateCommand.ExpectedVersion);
        }

        public void Save(TAgg aggregateRoot)
        {
            this.repository.Save(aggregateRoot);
        }

        #endregion
    }
}