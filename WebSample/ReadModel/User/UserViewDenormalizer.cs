namespace WebSample.ReadModel
{
    #region using

    using TheRing.CQRS.Eventing;

    using WebSample.Eventing;

    #endregion

    public class UserViewDenormalizer : Denormalizer, 
        IDenormalizeEvent<UserCreated>, 
        IDenormalizeEvent<UserAddressAdded>, 
        IDenormalizeEvent<UserConfirmed>
    {
        #region Constructors and Destructors

        public UserViewDenormalizer(IDenormalizerRepository repository) : base(repository)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void Consume(UserCreated @event)
        {
            this.Repository.Create<UserView>(
                u =>
                {
                    u.UserViewId = @event.EventSourcedId;
                    u.UserVersion = @event.EventSourcedVersion;
                    u.FirstName = @event.FirstName;
                    u.LastName = @event.LastName;
                    u.CanAddAddress = @event.CanAddAddress;
                });
        }

        public void Consume(UserAddressAdded @event)
        {
            this.Repository.Update<UserView>(
                @event.EventSourcedId, 
                u =>
                {
                    u.Addresses.Add(@event.Address);
                    u.UserVersion = @event.EventSourcedVersion;
                    u.CanAddAddress = @event.CanAddAddress;
                });
        }

        public void Consume(UserConfirmed @event)
        {
            this.Repository.Update<UserView>(
                @event.EventSourcedId,
                u =>
                {
                    u.Confirmed = true;
                    u.UserVersion = @event.EventSourcedVersion;
                });
        }

        #endregion
    }
}