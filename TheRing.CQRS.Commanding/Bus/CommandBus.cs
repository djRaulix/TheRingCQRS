using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.CQRS.Commanding.Bus
{
    public class CommandBus : ICommandBus
    {
        #region Implementation of ICommandBus

        public void Send<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            command.CorrelationId = correlationId;
        }

        public Response SendRequest<T>(T command, Guid correlationId) where T : AbstractCommand
        {
            command.CorrelationId = correlationId;
            command.ExpectResponse = true;
            return new Response("CommandNotSent","Command not sent");
        }

        #endregion
    }
}
