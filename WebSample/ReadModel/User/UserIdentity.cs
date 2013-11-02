namespace WebSample.ReadModel.User
{
    #region using

    using System;

    #endregion

    public class UserIdentity
    {
        #region Public Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid UserIdentityId { get; set; }

        #endregion
    }
}