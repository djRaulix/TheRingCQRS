using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.CQRS.Eventing.Test.Fakes
{
    using FakeItEasy;

    using TheRing.CQRS.Eventing.EventSourced;

    public class FakeConcreteEventSourced : AbstractEventSourced
    {
        public bool InternalStateModified { get; set; }

        public AbstractEvent EventThatDoesntModifyInternalState;
        public AbstractEvent EventThatModifyInternalState;
        
        public void MethodGeneratingEventThatDoesntModifyInternalState()
        {
            EventThatDoesntModifyInternalState = new FakeAnEvent();
            this.ApplyChange(EventThatDoesntModifyInternalState);
        }

        public void MethodGeneratingEventThatModifyInternalState()
        {
            EventThatModifyInternalState = new FakeAnOtherEvent();
            this.ApplyChange(EventThatModifyInternalState);
        }

        private void Apply(FakeAnOtherEvent @event)
        {
            InternalStateModified = true;
        }

        #region Overrides of AbstractEventSourced

        protected override void ApplyEvent(dynamic @event)
        {
            this.Apply(@event);
        }

        #endregion
    }
}
