namespace WebSample.Controllers
{
    #region using

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

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
            if(this.commandBus.SendOk(command))
            {
                return this.RedirectToAction("Details", new {command.Id});
            }
            throw new Exception("Error creating address");
            
        }

        [HttpPost]
        public ActionResult Create(CreateUserCommand command)
        {
            if (this.commandBus.SendOk(command))
            {
                return this.RedirectToAction("Index");
            }
            throw new Exception("Error creating user");
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