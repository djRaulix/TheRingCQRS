namespace WebSample.Commanding.User
{
    using TheRing.CQRS.Application;

    using WebSample.Domain.User;

    public class UserCommandRunner : IRunCommand<User,CreateUser>,
        IRunCommand<User, ConfirmUser>,
        IRunCommand<User, AddUserAddress>
    {
        #region Implementation of IRunCommand<in User,in CreateUser>

        public void Run(User user, CreateUser command)
        {
            user.Create(command.FirstName, command.LastName);
        }

        #endregion

        #region Implementation of IRunCommand<in User,in ConfirmUser>

        public void Run(User user, ConfirmUser command)
        {
            user.Confirm();
        }

        #endregion

        #region Implementation of IRunCommand<in User,in AddUserAddress>

        public void Run(User user, AddUserAddress command)
        {
            user.AddAddress(command.Address);
        }

        #endregion
    }
}