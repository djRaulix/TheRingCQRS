namespace WebSample.Commanding.User
{
    #region using

    using TheRing.CQRS.Application;

    using WebSample.Domain.User;

    #endregion

    public class UserCommandRunner : IRunCommandOnEventSourced<User, CreateUser>,
        IRunCommandOnEventSourced<User, ConfirmUser>,
        IRunCommandOnEventSourced<User, AddUserAddress>,
        IRunCommandOnEventSourced<User, DeleteUser>
    {
        #region Public Methods and Operators

        public void Run(User user, CreateUser command)
        {
            user.Create(command.FirstName, command.LastName);
        }

        public void Run(User user, ConfirmUser command)
        {
            user.Confirm();
        }

        public void Run(User user, AddUserAddress command)
        {
            user.AddAddress(command.Address);
        }

        #endregion

        #region Implementation of IRunCommandOnEventSourced<in User,in DeleteUser>

        public void Run(User eventSourced, DeleteUser command)
        {
            eventSourced.Delete();
        }

        #endregion
    }
}