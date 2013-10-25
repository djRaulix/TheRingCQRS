namespace TheRing.CQRS.Commanding
{
    #region using

    using System;

    #endregion

    public interface IAggregateRoot
    {
        void RunGeneric(ICommand command);
    }
}