namespace WebSample.ReadModel
{
    #region using

    using System.Linq;

    using Raven.Client.Indexes;

    #endregion

    public class UserIdentityTransformer : AbstractTransformerCreationTask<UserView>
    {
        #region Constructors and Destructors

        public UserIdentityTransformer()
        {
            this.TransformResults = users => from user in users
                select new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName
                };
        }

        #endregion
    }
}