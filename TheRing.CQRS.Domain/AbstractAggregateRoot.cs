namespace TheRing.CQRS.Domain
{
    #region using

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Runner;
    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.EventSourced;

    #endregion

    public abstract class AbstractAggregateRoot : AbstractEventSourced, ICommandRunner
    {
        #region Public Methods and Operators

        public void RunGeneric(ICommand command)
        {
            this.CurrentCorrelationId = command.CorrelationId;
            ((dynamic)this).Run((dynamic)command);
        }

        #endregion
    }
}