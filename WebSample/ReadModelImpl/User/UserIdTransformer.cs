namespace WebSample.ReadModelImpl.User
{
    using System.Linq;

    using Raven.Client.Indexes;

    using WebSample.ReadModel;
    using WebSample.ReadModel.User;

    public class UserIdTransformer : AbstractTransformerCreationTask<UserView>
    {
        #region Constructors and Destructors

        public UserIdTransformer()
        {
            this.TransformResults = users => from user in users
                select new { Id = user.UserViewId };
        }

        #endregion
    }
}