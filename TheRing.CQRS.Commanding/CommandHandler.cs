namespace TheRing.CQRS.Commanding
{

    #region using

    #endregion

    public class CommandHandler<TAgg, TCommand> : IHandlesCommand<TCommand>
        where TCommand : class, ICommand
        where TAgg : IAggregateRoot
    {
        #region Fields

        private readonly IAggregateRootRepository<TAgg> repository;

        #endregion

        #region Constructors and Destructors

        public CommandHandler(IAggregateRootRepository<TAgg> repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Public Methods and Operators

        public void Handles(IHandlesContext<TCommand> handlesContext)
        {
            var command = handlesContext.Command;
            TAgg agg;
            try
            {
                var updateCommand = command as UpdateCommand;
                agg = updateCommand == null
                    ? this.repository.Create(command.Id)
                    : this.repository.Get(command.Id, updateCommand.ExpectedVersion);
            }
            catch (AggregateRootConcurrencyException)
            {
                if (command.ExpectResponse)
                {
                    handlesContext.Respond(new ConcurrencyExceptionResponse());
                    return;
                }
                throw;
            }

            agg.RunGeneric(command);

            try
            {
                this.repository.Save(agg);
            }
            catch (AggregateRootConcurrencyException)
            {
                if (handlesContext.RetryLater())
                {
                    return;
                }

                if (command.ExpectResponse)
                {
                    handlesContext.Respond(new ConcurrencyExceptionResponse());
                    return;
                }

                throw;
            }

            if (command.ExpectResponse)
            {
                handlesContext.Respond(new DoneResponse());
            }
        }

        #endregion
    }
}