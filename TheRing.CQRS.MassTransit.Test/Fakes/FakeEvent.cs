using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.CQRS.MassTransit.Test.Fakes
{
    using System.Threading;

    using TheRing.CQRS.Eventing;

    public class FakeEvent : Event
    {
        public FakeEvent()
        {
            this.CorrelationId = Guid.NewGuid();
        }
    }
}
