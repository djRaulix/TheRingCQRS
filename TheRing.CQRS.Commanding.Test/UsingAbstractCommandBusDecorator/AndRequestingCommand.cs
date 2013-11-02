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

        protected override void Because_of()
        {
            base.Because_of();
            this.Decorator.SendRequest(this.Command,this.CorrelationId);
        }

        protected override void Establish_context()
        {
            base.Establish_context();

            this.Decorator.CallsTo(d => d.SendRequest(this.Command, this.CorrelationId)).CallsBaseMethod();
        }

        #endregion
    }
}