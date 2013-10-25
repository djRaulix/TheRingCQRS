namespace WebSample.Eventing
{
    using TheRing.CQRS.Eventing;

    public class UserAddressAdded : Event
    {
        #region Public Properties

        public string Address { get; set; }

        public bool CanAddAddress { get; set; }

        #endregion
    }
}