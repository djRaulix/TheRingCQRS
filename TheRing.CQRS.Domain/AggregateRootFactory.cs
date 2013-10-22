namespace TheRing.CQRS.Domain
{
    using System;

    public class AggregateRootFactory : IAggregateRootFactory
    {
        #region Fields

        private readonly Func<Type, object> objectFactory;

        #endregion

        #region Constructors and Destructors

        public AggregateRootFactory(Func<Type,object> objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        #endregion

        #region Public Methods and Operators

        public T Create<T>() where T : AggregateRoot
        {
            return (T)this.objectFactory(typeof(T));
        }

        #endregion
    }
}