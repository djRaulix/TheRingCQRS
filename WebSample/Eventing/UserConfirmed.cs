namespace WebSample.Eventing
{
    #region using

    using TheRing.CQRS.Eventing;

    #endregion

    public class UserConfirmed : Event
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