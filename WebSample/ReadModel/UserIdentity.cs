﻿namespace WebSample.ReadModel
{
    #region using

    using System;

    #endregion

    public class UserIdentity
    {
        #region Public Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid UserId { get; set; }

        #endregion
    }
}