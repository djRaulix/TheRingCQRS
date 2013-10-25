namespace TheRing.CQRS.Domain
{
    #region using

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Eventing;

    #endregion

    public abstract class AggregateRoot : EventSourced, IAggregateRoot
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