namespace TheRing.CQRS.Commanding.Bus
{
    #region using

    using System;

    #endregion

    public class Response
    {
        #region Constructors and Destructors

        public Response()
        {
        }
        
        public Response(bool ok)
        {
            this.Ok = ok;
        }

        public Response(Exception ex) : this(false)
        {
            this.ErrorType = ex.GetType().Name;
            this.ErrorMessage = ex.Message;
        }

        public Response(string errorType, string errorMessage) : this(false)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        #endregion

        #region Public Properties

        public string ErrorMessage { get; private set; }

        public string ErrorType { get; private set; }

        public bool Ok { get; private set; }

        #endregion
    }
}