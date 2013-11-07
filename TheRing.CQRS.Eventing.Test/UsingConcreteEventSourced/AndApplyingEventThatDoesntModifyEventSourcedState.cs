namespace TheRing.CQRS.Eventing.Test.UsingConcreteEventSourced
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndApplyingEventThatDoesntModifyEventSourcedState : UsingConcreteEventSourced
    {
        #region Public Methods and Operators

        [Test]
        public void ThenEventShouldbeListedInChanges()
        {
            this.EventSourced.Changes.Should().Contain(this.EventSourced.EventThatDoesntModifyInternalState);
        }

        [Test]
        public void ThenInternalStateShouldNotBeModified()
        {
            this.EventSourced.InternalStateModified.Should().BeFalse();
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventSourced.MethodGeneratingEventThatDoesntModifyInternalState();
        }

        #endregion
    }
}