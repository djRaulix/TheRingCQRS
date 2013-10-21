namespace WebSample.ReadModel
{
    #region using

    using System;

    using TheRing.CQRS.Eventing;

    using WebSample.Eventing;

    #endregion

    public class UserViewDenormalizer : Denormalizer, IDenormalizeEvent<UserCreated>,
        IDenormalizeEvent<UserAddressAdded>,
        IDenormalizeEvent<UserConfirmed>
    {
        #region Public Methods and Operators

        public UserViewDenormalizer(IDenormalizerRepository repository) : base(repository)
        {
        }

        public void Consume(UserCreated @event)
        {
            Repository.Create<UserView>(
                u =>
                {
                    u.UserId = @event.EventSourcedId;
                    u.UserVersion = @event.EventSourcedVersion;
                    u.FirstName = @event.FirstName;
                    u.LastName = @event.LastName;
                });
        }

        public void Consume(UserAddressAdded @event)
        {
            Repository.Update<UserView>(@event.EventSourcedId,
                u =>
                {
                    u.Addresses.Add(@event.Address);
                    u.UserVersion = @event.EventSourcedVersion;
                });
        }

        #endregion

        #region Implementation of IDenormalizeEvent<in UserConfirmed>

        public void Consume(UserConfirmed @event)
        {
            Repository.Update<UserView>(@event.EventSourcedId,
                u =>
                {
                    u.Confirmed = true;
                });
        }

        #endregion
    }
}