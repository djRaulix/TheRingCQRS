namespace WebSample.ReadModel
{
    #region using

    using System;

    using Magnum.Collections;

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