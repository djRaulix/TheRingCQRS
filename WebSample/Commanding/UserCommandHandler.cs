namespace WebSample.Commanding
{
    #region using

    using TheRing.CQRS.Commanding;
    using WebSample.Domain;

    #endregion

    public class UserCommandHandler : IRunCommand<User, CreateUserCommand>,
        IRunCommand<User, AddUserAddressCommand>,
        IRunCommand<User, ConfirmUserCommand>
    {
        #region Implementation of IRunCommand<in User,in CreateUserCommand>

        public void Run(User aggregateRoot, CreateUserCommand command)
        {
            aggregateRoot.Create(command.FirstName, command.LastName);
        }

        #endregion

        #region Implementation of IRunCommand<in User,in AddUserAddressCommand>

        public void Run(User aggregateRoot, AddUserAddressCommand command)
        {
            aggregateRoot.AddAddress(command.Address);
        }

        #endregion

        #region Implementation of IRunCommand<in User,in ConfirmUserCommand>

        public void Run(User aggregateRoot, ConfirmUserCommand command)
        {
            aggregateRoot.Confirm();
        }

        #endregion
    }
}