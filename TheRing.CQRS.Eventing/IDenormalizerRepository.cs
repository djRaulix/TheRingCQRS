namespace TheRing.CQRS.Eventing
{
    using System;

    public interface IDenormalizerRepository
    {
        #region Public Methods and Operators

        void Create<T>(Action<T> initialize) where T : new();

        void Update<T>(Guid id, Action<T> update) where T : new();

        #endregion
    }
}