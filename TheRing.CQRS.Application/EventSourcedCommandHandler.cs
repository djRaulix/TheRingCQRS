namespace TheRing.CQRS.Application
{
    #region using

    using System;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.Commanding.Handler;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.EventSourced;
    using TheRing.CQRS.Eventing.EventSourced.Repository;

    #endregion

    public class EventSourcedCommandHandler<TEventSourced, TCommand> : IHandleCommand<TCommand>
        where TEventSourced : AbstractEventSourced
        where TCommand : AbstractEditCommand
    {
        #region Fields

        private readonly IEventSourcedRepository repository;
        private readonly IRunCommandOnEventSourced<TEventSourced, TCommand> runner;

        #endregion

        #region Constructors and Destructors

        public EventSourcedCommandHandler(
            IRunCommandOnEventSourced<TEventSourced, TCommand> runner,
            IEventSourcedRepository repository)
        {
            this.runner = runner;
            this.repository = repository;
        }

        #endregion

        #region Public Methods and Operators

        public Response Handle(TCommand command)
        {
            TEventSourced eventSourced;

            if (command is AbstractCreateCommand)
            {
                eventSourced = this.repository.Create<TEventSourced>(command.Id);
            }
            else
            {
                eventSourced = this.repository.Get<TEventSourced>(command.Id);
                eventSourced.CurrentCorrelationId = command.CorrelationId;

                if (!(command.ExpectedVersion == null || command.ExpectedVersion == eventSourced.Version))
                {
                    return
                        new Response(
                            "NotExpectedVersion",
                            string.Format(
                                "{0} (version : {1}) not in expected version ({2})",
                                eventSourced.GetType().Name,
                                eventSourced.Version,
                                command.ExpectedVersion));
                }
            }

            try
            {
                this.runner.Run(eventSourced, command);
            }
            catch (EventingException ex)
            {
                return new Response(ex);
            }
            catch (CommandingException ex)
            {
                return new Response(ex);
            }

            this.repository.Save(eventSourced);
            return new Response(true);
        }

        #endregion
    }
}