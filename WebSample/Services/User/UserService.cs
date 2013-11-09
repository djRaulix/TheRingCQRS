namespace WebSample.Services.User
{
    #region using

    using TheRing.CQRS.Commanding.Bus;
    using TheRing.CQRS.Commanding.Handler;

    using WebSample.Commanding.User;
    using WebSample.ReadModel;
    using WebSample.ReadModel.User;

    #endregion

    public class UserService : IRunCommand<DeleteAllUsers>
    {
        #region Fields

        private readonly ICommandBus commandBus;
        private readonly IUserRepository userRepository;

        #endregion

        #region Constructors and Destructors

        public UserService(IUserRepository userRepository, ICommandBus commandBus)
        {
            this.userRepository = userRepository;
            this.commandBus = commandBus;
        }

        #endregion

        #region Public Methods and Operators

        public void Run(DeleteAllUsers command)
        {
            foreach (var id in this.userRepository.GetAllUserIds())
            {
                this.commandBus.Send(new DeleteUser { Id = id });
            }
        }

        #endregion
    }
}