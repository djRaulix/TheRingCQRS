namespace TheRing.CQRS.Eventing.Test.UsingConcreteEventSourced
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndApplyingEventThatModifyEventSourcedState : UsingConcreteEventSourced
    {
        #region Public Methods and Operators

        [Test]
        public void ThenEventShouldbeListedInChanges()
        {
            this.EventSourced.Changes.Should().Contain(this.EventSourced.EventThatModifyInternalState);
        }

        [Test]
        public void ThenInternalStateShouldBeModified()
        {
            this.EventSourced.InternalStateModified.Should().BeTrue();
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            this.EventSourced.MethodGeneratingEventThatModifyInternalState();
        }

        #endregion
    }
}