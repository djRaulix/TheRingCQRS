namespace TheRing.CQRS.Commanding
{
    #region using

    using TheRing.CQRS.Domain;

    #endregion

    public class CommandHandler<TAgg, TCommand> : IHandlesCommand<TCommand>
        where TCommand : class, ICommand
        where TAgg : IAggregateRoot
    {
        #region Fields

        private readonly IEditAggregate<TAgg> aggregateEditor;
        private readonly IRunCommand<TAgg, TCommand> commandRunner;

        #endregion

        #region Constructors and Destructors

        public CommandHandler(IEditAggregate<TAgg> aggregateEditor, IRunCommand<TAgg, TCommand> commandRunner)
        {
            this.aggregateEditor = aggregateEditor;
            this.commandRunner = commandRunner;
        }

        #endregion

        #region Public Methods and Operators

        public void Handles(IConsumesContext<TCommand> consumesContext)
        {
            var command = consumesContext.Command;
            TAgg agg;
            try
            {
                agg = this.aggregateEditor.Get(command);
            }
            catch (AggregateRootConcurrencyException)
            {
                if (command.ExpectResponse)
                {
                    consumesContext.Respond(new ConcurrencyExceptionResponse());
                    return;
                }
                throw;
            }

            agg.SetCurrentCorrelationId(command.CorrelationId);

            this.commandRunner.Run(agg, command);

            try
            {
                this.aggregateEditor.Save(agg);
            }
            catch (AggregateRootConcurrencyException)
            {
                if (consumesContext.RetryLater())
                {
                    return;
                }

                if (command.ExpectResponse)
                {
                    consumesContext.Respond(new ConcurrencyExceptionResponse());
                    return;
                }

                throw;
            }

            if (command.ExpectResponse)
            {
                consumesContext.Respond(new DoneResponse());
            }
        }

        #endregion
    }
}