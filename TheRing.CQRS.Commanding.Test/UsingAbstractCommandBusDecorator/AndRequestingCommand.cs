namespace TheRing.CQRS.Commanding.Test.UsingAbstractCommandBusDecorator
{
    #region using

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using NUnit.Framework;

    #endregion

    public class AndRequestingCommand : UsingAbstractCommandBusDecorator
    {
        #region Public Methods and Operators

        [Test]
        public void ThenDecoratedBusShouldRequestCommand()
        {
            this.CommandBus.CallsTo(b => b.SendRequest(this.Command,this.CorrelationId)).MustHaveHappened(Repeated.Exactly.Once);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.Decorator.SendRequest(this.Command,this.CorrelationId);
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();

            this.Decorator.CallsTo(d => d.SendRequest(this.Command, this.CorrelationId)).CallsBaseMethod();
        }

        #endregion
    }
}