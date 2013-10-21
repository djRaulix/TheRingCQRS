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
        #region Public Methods and Operators

        public void Run(User aggregateRoot, CreateUserCommand command)
        {
            aggregateRoot.Create(command.FirstName, command.LastName);
        }

        public void Run(User aggregateRoot, AddUserAddressCommand command)
        {
            aggregateRoot.AddAddress(command.Address);
        }

        public void Run(User aggregateRoot, ConfirmUserCommand command)
        {
            aggregateRoot.Confirm();
        }

        #endregion
    }
}