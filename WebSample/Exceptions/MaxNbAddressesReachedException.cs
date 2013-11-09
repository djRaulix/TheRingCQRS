using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSample.Exceptions
{
    using TheRing.CQRS.Eventing;

    public class MaxNbAddressesReachedException : EventingException
    {
        public MaxNbAddressesReachedException() : base("can't add another address")
        {
            
        }
    }
}