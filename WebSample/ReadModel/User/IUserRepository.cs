namespace WebSample.ReadModel
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    public interface IUserRepository
    {
        #region Public Methods and Operators

        IEnumerable<UserIdentity> GetUserIdentities(
            int page, 
            int nbByPage, 
            out int totalNb, 
            Expression<Func<UserIdentity, object>> orderByProperty = null, 
            bool asc = true);

        UserView GetUserView(Guid id);

        #endregion
    }
}