namespace WebSample.Domain.User
{
    using TheRing.CQRS.Eventing;

    public class UserAddressAdded : AbstractEvent
    {
        #region Public Properties

        public string Address { get; set; }

        public bool CanAddAddress { get; set; }

        #endregion
    }
}