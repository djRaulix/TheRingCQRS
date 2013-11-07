namespace WebSample.Domain.User
{
    #region using

    using TheRing.CQRS.Eventing.EventSourced;

    using WebSample.Exceptions;

    #endregion

    public class User : AbstractSnapshotableEventSourced
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
            if (this.nbAddresses >= MaxAddresses)
            {
                throw new MaxNbAddressesReachedException();
            }
            this.ApplyChange(
                new UserAddressAdded
                {
                    Address = address,
                    CanAddAddress = this.nbAddresses < MaxAddresses - 1
                });
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

        public void Delete()
        {
            this.ApplyChange(new UserDeleted());
        }

        #endregion

        #region Methods

        internal void Apply(UserAddressAdded @event)
        {
            this.nbAddresses++;
        }

        protected override object GetSnapshot()
        {
            return this.nbAddresses;
        }

        protected override void RestoreFromSnapshot(object snapshot)
        {
            this.nbAddresses = (int)snapshot;
        }

        #endregion
    }
}