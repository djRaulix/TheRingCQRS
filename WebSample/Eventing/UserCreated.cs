namespace WebSample.Eventing
{
    #region using

    using TheRing.CQRS.Eventing;

    #endregion

    public class UserCreated : Event
    {
        #region Public Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override bool Volatile
        {
            get
            {
                return true;
            }
        }

        public bool CanAddAddress { get; set; }

        #endregion
    }
}