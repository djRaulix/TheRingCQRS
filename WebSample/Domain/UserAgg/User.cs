namespace WebSample.Domain.UserAgg
{
    #region using

    using TheRing.CQRS.Domain;

    using WebSample.Commanding;
    using WebSample.Eventing;

    #endregion

    public class User : AggregateRoot
    {
        #region Constants

        private const int MaxAddresses = 5;

        #endregion

        #region Fields

        private int nbAddresses;

        #endregion

        #region Public Methods and Operators

        public void Apply(UserAddressAdded @event)
        {
            this.nbAddresses++;
        }

        public void Run(ConfirmUser command)
        {
            this.ApplyChange(new UserConfirmed());
        }

        public void Run(AddUserAddress command)
        {
            if (MaxAddresses > this.nbAddresses)
            {
                this.ApplyChange(
                    new UserAddressAdded
                    {
                        Address = command.Address,
                        CanAddAddress = this.nbAddresses < MaxAddresses - 1
                    });
            }
        }

        public void Run(CreateUser command)
        {
            this.ApplyChange(
                new UserCreated
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    CanAddAddress = true
                });
        }

        #endregion

        #region Methods

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