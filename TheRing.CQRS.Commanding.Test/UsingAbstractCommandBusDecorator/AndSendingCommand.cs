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

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.Decorator.Send(this.Command,this.CorrelationId);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.Decorator.CallsTo(d => d.Send(this.Command, this.CorrelationId)).CallsBaseMethod();
        }

        #endregion
    }
}