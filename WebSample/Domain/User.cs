namespace WebSample.Domain
{
    #region using

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.EventSourcedDomain;

    using WebSample.Eventing;

    #endregion

    public class User : EventSourcedAggregateRoot
    {
        #region Constants

        private const int MaxAddresses = 5;

        #endregion

        #region Fields

        private int nbAddresses;

        #endregion

        #region Public Methods and Operators

        public void AddAddress(string address)
        {
            if (MaxAddresses > this.nbAddresses)
            {
                this.ApplyChange(
                    new UserAddressAdded
                    {
                        Address = address,
                        CanAddAddress = this.nbAddresses < MaxAddresses - 1
                    });
            }
        }

        public void Confirm()
        {
            this.ApplyChange(new UserConfirmed());
        }

        public void Create(string firstName, string lastName)
        {
            this.ApplyChange(
                new UserCreated
                {
                    FirstName = firstName,
                    LastName = lastName,
                    CanAddAddress = true
                });
        }

        #endregion

        #region Methods

        protected override void ApplyGeneric(Event @event)
        {
            this.Apply((dynamic)@event);
        }

        private void Apply(UserAddressAdded @event)
        {
            this.nbAddresses++;
        }

        #endregion
    }
}