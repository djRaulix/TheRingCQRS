namespace TheRing.CQRS.Eventing.Denormalizer
{
    #region using

    using System;

    #endregion

    public interface IDenormalizerRepository
    {
        #region Public Methods and Operators

        void Create<T>(Action<T> initialize) where T : new();

        void Update<T>(string id, Action<T> update) where T : new();

        void Delete<T>(string id) where T : new();

        void Update<T>(ValueType id, Action<T> update) where T : new();

        void Delete<T>(ValueType id) where T : new();

        #endregion
    }
}