namespace WebSample.Controllers
{
    #region using

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using TheRing.CQRS.Commanding;
    using TheRing.CQRS.Commanding.Bus;

    using WebSample.Commanding.User;
    using WebSample.ReadModel;
    using WebSample.ReadModel.User;
    using WebSample.Services.User;

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
        public ActionResult AddAddress(AddUserAddress command)
        {
            var result = this.commandBus.SendRequest(command);
            if (result.Ok)
            {
                return this.RedirectToAction("Details", new {command.Id});
            }
            throw new Exception(result.ErrorMessage);
            
        }

        [HttpPost]
        public ActionResult Create(CreateUser command)
        {
            if (this.commandBus.SendRequest(command).Ok)
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

        public ActionResult DeleteAll()
        {
            this.commandBus.Send(new DeleteAllUsers());
            return this.RedirectToAction("Index");
        }

        #endregion
    }
}