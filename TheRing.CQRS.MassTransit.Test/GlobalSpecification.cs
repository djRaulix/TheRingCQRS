namespace TheRing.CQRS.RavenDb.Test
{
    #region using

    using System;

    using FakeItEasy;

    using TheRing.CQRS.MassTransit;

    #endregion

    public sealed class GlobalSpecification
    {
        #region Static Fields

        private static readonly object LockObject = new Object();
        private static IBusFactory busFactory;

        #endregion

        #region Public Properties

        public static IBusFactory BusFactory
        {
            get
            {
                if (busFactory == null)
                {
                    lock (LockObject)
                    {
                        if (busFactory == null)
                        {
                            busFactory = new BusFactory(
                                type =>
                                {
                                    var method = typeof(A).GetMethod("Fake");
                                    var generic = method.MakeGenericMethod(type);
                                    return generic.Invoke(null, null);
                                });
                            busFactory.Set("loopback://localhost/queue");
                        }
                    }
                }
                return busFactory;
            }
        }

        #endregion
    }
}