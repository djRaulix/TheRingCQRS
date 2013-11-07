using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.CQRS.Commanding.Test.UsingCommandHandler
{
    using FakeItEasy;

    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.Commanding.Handler;
    using TheRing.CQRS.Commanding.Test.Fakes;

    public abstract class UsingCommandHandler : Specification
    {
        protected CommandHandler<FakeCommand> CommandHandler;

        protected IRunCommand<FakeCommand> CommandRunner;

        protected FakeCommand Command;

        protected Response Response;

        #region Overrides of SpecBase

        protected override void EstablishContext()
        {
            base.EstablishContext();
            CommandRunner = A.Fake<IRunCommand<FakeCommand>>();
            CommandHandler = new CommandHandler<FakeCommand>(CommandRunner);
            Command = new FakeCommand();
        }

        #endregion
    }
}
