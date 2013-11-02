namespace TheRing.CQRS.Commanding.Test.UsingAbstractCommandBusDecorator
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    #endregion

    public class AndSendingCommand : UsingAbstractCommandBusDecorator
    {
        #region Public Methods and Operators

        [Test]
        public void ThenDecoratedBusShouldSendCommand()
        {
            this.CommandBus.CallsTo(b => b.Send(this.Command,this.CorrelationId)).MustHaveHappened(Repeated.Exactly.Once);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.Decorator.Send(this.Command,this.CorrelationId);
        }

        protected override void Establish_context()
        {
            base.Establish_context();

            this.Decorator.CallsTo(d => d.Send(this.Command, this.CorrelationId)).CallsBaseMethod();
        }

        #endregion
    }
}