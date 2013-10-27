namespace WebSample.Domain.User
{
    #region using

    using TheRing.CQRS.Eventing;
    using TheRing.CQRS.Eventing.EventSourced;

    #endregion

    public class User : AbstractEventSourced
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

        protected override void ApplyGeneric(AbstractEvent @event)
        {
            this.Apply((dynamic)@event);
        }

        protected override object GetSnapshot()
        {
            return this.nbAddresses;
        }

        protected override void RestoreFromSnapshot(object snapshot)
        {
            this.nbAddresses = (int)snapshot;
        }

        private void Apply(UserAddressAdded @event)
        {
            this.nbAddresses++;
        }

        #endregion
    }
}