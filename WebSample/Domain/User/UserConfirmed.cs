namespace WebSample.Domain.User
{
    #region using

    using TheRing.CQRS.Eventing;

    #endregion

    public class UserConfirmed : AbstractEvent
    {
        #region Public Properties

        public override bool Volatile
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}