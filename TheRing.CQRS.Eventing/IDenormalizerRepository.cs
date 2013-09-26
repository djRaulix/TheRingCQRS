namespace TheRing.CQRS.Eventing
{
    #region using

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IDenormalizerRepository
    {
        #region Public Methods and Operators

        void Create<T>(Action<T> initialize) where T : new();

        void Update<T>(object id, Action<T> update) where T : new();

        void Delete<T>(object id) where T : new();

        void Update<T>(Expression<Func<T,bool>> filter, Action<T> update) where T : new();

        void Delete<T>(Expression<Func<T,bool>> filter) where T : new();

        #endregion
    }
}