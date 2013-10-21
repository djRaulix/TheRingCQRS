namespace WebSample.Domain
{
    #region using

    using TheRing.CQRS.Domain;
    using TheRing.CQRS.Eventing;

    using WebSample.Eventing;

    #endregion

    public class User : AggregateRoot
    {
        #region Constants

        private const int maxAddresses = 5;

        #endregion

        #region Fields

        private int nbAddresses;

        #endregion

        #region Public Methods and Operators

        public void AddAddress(string address)
        {
            if (maxAddresses > this.nbAddresses)
            {
                this.ApplyChange(
                    new UserAddressAdded
                    {
                        Address = address,
                        CanAddAddress = this.nbAddresses < maxAddresses - 1
                    });
            }
        }

        private void Apply(UserAddressAdded @event)
        {
            this.nbAddresses++;
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

        public void Confirm()
        {
            this.ApplyChange(new UserConfirmed());
        }

        #endregion

        #region Methods

        protected override void ApplyGeneric(Event @event)
        {
            this.Apply((dynamic)@event);
        }

        #endregion
    }
}