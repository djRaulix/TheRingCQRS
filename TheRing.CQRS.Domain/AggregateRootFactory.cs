namespace TheRing.CQRS.Domain
{
    using System;

    public class AggregateRootFactory : IAggregateRootFactory
    {
        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion

        #region Constructors and Destructors

        public AggregateRootFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods and Operators

        public T Create<T>() where T : AggregateRoot
        {
            return (T)this.serviceProvider.GetService(typeof(T));
        }

        #endregion
    }
}