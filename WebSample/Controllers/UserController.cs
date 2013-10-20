namespace WebSample.Controllers
{
    #region using

    using System;
    using System.Web.Mvc;

    using TheRing.CQRS.Commanding;

    using WebSample.Commanding;
    using WebSample.ReadModel;

    #endregion

    public class UserController : Controller
    {
        #region Fields

        private readonly ICommandBus commandBus;
        private readonly IUserRepository repository;

        #endregion

        #region Constructors and Destructors

        public UserController(IUserRepository repository, ICommandBus commandBus)
        {
            this.repository = repository;
            this.commandBus = commandBus;
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        public ActionResult AddAddress(AddUserAddressCommand command)
        {
            this.commandBus.Send(command);
            return this.RedirectToAction("Details");
        }

        [HttpPost]
        public ActionResult Create(CreateUserCommand command)
        {
            this.commandBus.Send(command);
            return this.RedirectToAction("Index");
        }

        public ActionResult Details(Guid id)
        {
            return this.View(this.repository.GetUserView(id));
        }

        public ActionResult Index()
        {
            int nbUsers;
            return this.View(this.repository.GetUserIdentities(1, 20, out nbUsers));
        }

        #endregion
    }
}