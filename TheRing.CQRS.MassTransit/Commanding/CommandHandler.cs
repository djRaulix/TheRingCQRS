namespace TheRing.CQRS.MassTransit.Commanding
{
    #region using

    using global::MassTransit;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Domain;

    #endregion

    public class CommandHandler<TAgg, TCommand> : Consumes<TCommand>.Context
        where TCommand : class, ICommand where TAgg : AggregateRoot
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

        public void Consume(IConsumeContext<TCommand> context)
        {
            var message = context.Message;
            TAgg agg;
            const int MaxTries = 3;

            try
            {
                agg = this.aggregateEditor.Get(message);
            }
            catch (DomainConcurrencyException)
            {
                if (message.ExpectResponse)
                {
                    context.Respond(new ConcurrencyExceptionResponse());
                    return;
                }

                throw;
            }

            agg.SetCurrentCorrelationId(message.CorrelationId);

            this.commandRunner.Run(agg, message);

            try
            {
                this.aggregateEditor.Save(agg);
            }
            catch (DomainConcurrencyException)
            {
                if (context.RetryCount < MaxTries)
                {
                    context.RetryLater();
                    return;
                }

                if (message.ExpectResponse)
                {
                    context.Respond(new ConcurrencyExceptionResponse());
                    return;
                }

                throw;
            }

            if (message.ExpectResponse)
            {
                context.Respond(new DoneResponse());
            }
        }

        #endregion
    }
}