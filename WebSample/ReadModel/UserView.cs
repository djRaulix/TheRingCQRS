namespace WebSample.ReadModel
{
    using System;
    using System.Collections.Generic;

    public class UserView
    {
        public UserView()
        {
            Addresses = new List<string>();
        }

        public Guid UserId { get; set; }

        public int UserVersion { get; set; }
        
        #region Public Properties

        public ICollection<string> Addresses { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Confirmed { get; set; }

        #endregion
    }
}