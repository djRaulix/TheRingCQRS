namespace WebSample.Domain.User
{
    #region using

    using TheRing.CQRS.Eventing;

    #endregion

    public class UserCreated : AbstractEvent
    {
        #region Public Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool CanAddAddress { get; set; }

        #endregion
    }
}