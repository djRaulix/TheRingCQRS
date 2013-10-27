namespace WebSample.Commanding.User
{
    #region using

    using TheRing.CQRS.Application;

    #endregion

    public class AddUserAddress : AbstractEditCommand
    {
        #region Public Properties

        public string Address { get; set; }

        #endregion
    }
}