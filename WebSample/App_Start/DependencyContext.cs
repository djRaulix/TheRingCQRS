namespace WebSample.App_Start
{
    #region using

    using System;
    using System.Diagnostics;

    #endregion

    [DebuggerDisplay(
        "DependencyContext (ServiceType: {ServiceType}, " +
        "ImplementationType: {ImplementationType})")]
    public class DependencyContext
    {
        #region Static Fields

        internal static readonly DependencyContext Root =
            new DependencyContext();

        #endregion

        #region Constructors and Destructors

        internal DependencyContext(
            Type serviceType, 
            Type implementationType)
        {
            this.ServiceType = serviceType;
            this.ImplementationType = implementationType;
        }

        private DependencyContext()
        {
        }

        #endregion

        #region Public Properties

        public Type ImplementationType { get; private set; }

        public Type ServiceType { get; private set; }

        #endregion
    }
}