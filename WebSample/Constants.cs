using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSample
{
    public class Constants
    {
        #region Constants

        private const string BusAddress = "rabbitmq://localhost/";

        #endregion

        #region Public Properties

        public static string RequestQueue
        {
            get { return BusAddress + "Requests"; }
        }

        public static string ResponseQueue
        {
            get { return BusAddress + "Responses"; }
        }

        public static string ReadModelQueue
        {
            get { return BusAddress + "ReadModel"; }
        }

        public static string SagaQueue
        {
            get { return BusAddress + "Sagas"; }
        }

        #endregion
    }
}