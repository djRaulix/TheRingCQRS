namespace TheRing.CQRS.Domain.UnitTests.WhenUsingTheRepository
{
    #region using

    using System;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using TheRing.CQRS.Domain.UnitTests.Fakes;
    using TheRing.CQRS.Eventing;

    #endregion

    public abstract class WhenUsingTheRepository : Specification
    {
        #region Properties

        protected FakeAggregate AggregateRoot { get; set; }

        protected IAggregateRootFactory AggregateRootFactory { get; set; }

        protected IEventStore EventStore { get; set; }

        protected Guid Id { get; set; }

        protected Repository Repository { get; set; }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            this.Repository = new Repository(this.EventStore, this.AggregateRootFactory);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.Id = Guid.NewGuid();
            this.EventStore = A.Fake<IEventStore>();
            this.AggregateRoot = A.Fake<FakeAggregate>();
            this.AggregateRootFactory = A.Fake<IAggregateRootFactory>();
            this.AggregateRootFactory.CallsTo(f => f.Create<FakeAggregate>()).Returns(this.AggregateRoot);
            
        }

        #endregion
    }
}