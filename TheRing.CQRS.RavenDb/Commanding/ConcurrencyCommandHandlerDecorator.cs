namespace TheRing.CQRS.RavenDb.Commanding
{
    #region using

    using Raven.Abstractions.Exceptions;

    using TheRing.CQRS.Application;
    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.Commanding.Handler;
    using TheRing.CQRS.RavenDb.Properties;

    #endregion

    public class ConcurrencyCommandHandlerDecorator<TCommand> : AbstractCommandHandlerDecorator<TCommand>
        where TCommand : AbstractEditCommand
    {
        #region Constructors and Destructors

        public ConcurrencyCommandHandlerDecorator(IHandleCommand<TCommand> handler) : base(handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override Response Handle(TCommand command)
        {
            var nbTries = 1;
            while (true)
            {
                try
                {
                    return base.Handle(command);
                }
                catch (ConcurrencyException ex)
                {
                    if (command is AbstractCreateCommand)
                    {
                        return new Response(
                            "PersistedObjectAlreadyExist",
                            string.Format("Object with{0}: AlreadyExists", command.Id));
                    }
                    if (nbTries >= Settings.Default.NbTriesOnConcurrency)
                    {
                        return new Response(ex);
                    }
                    nbTries++;
                }
            }
        }

        #endregion
    }
}