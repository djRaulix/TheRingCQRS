namespace TheRing.CQRS.Commanding.Test.UsingAbstractCommandBusDecorator
{
    using System;

    using FakeItEasy;

    using TheRing.CQRS.Commanding.Bus;

    public abstract class UsingAbstractCommandBusDecorator : Specification
    {
        protected AbstactCommandBusDecorator Decorator;

        protected ICommandBus CommandBus;
        
        protected AbstractCommand Command;

        protected Guid CorrelationId;

        #region Overrides of SpecBase

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.Command = A.Fake<AbstractCommand>();
            this.CorrelationId = Guid.NewGuid();
            this.CommandBus = A.Fake<ICommandBus>();
            this.Decorator = A.Fake<AbstactCommandBusDecorator>(o => o.WithArgumentsForConstructor(new []{this.CommandBus}));
        }

        #endregion
    }
}
