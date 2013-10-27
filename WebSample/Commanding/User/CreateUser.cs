namespace WebSample.Commanding.User
{
    #region using

    using TheRing.CQRS.Application;

    #endregion

    public class CreateUser : AbstractCreateCommand
    {
        #region Public Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        #endregion
    }
}