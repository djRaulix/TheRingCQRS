using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRing.CQRS.Commanding
{
    public class CommandingException : Exception
    {
        public CommandingException(string message)
            : base(message)
        {
            
        }
    }
}
