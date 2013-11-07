using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.CQRS.Commanding.Test.UsingCommandBus
{
    using FakeItEasy;

    using TheRing.CQRS.Commanding.Bus;

    public abstract class UsingCommandBus : Specification
    {
        protected CommandBus CommandBus;

        protected AbstractCommand Command;

        protected Guid CorrelationId;
        
        #region Overrides of SpecBase

        protected override void EstablishContext()
        {
            base.EstablishContext();
            CommandBus = new CommandBus();
            Command = A.Fake<AbstractCommand>();
            CorrelationId = Guid.NewGuid();
        }

        #endregion
    }
}
