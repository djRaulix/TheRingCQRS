namespace TheRing.MassTransit.RavenDb.Test.Fakes
{
    #region using

    using System;

    using global::MassTransit;
    using global::MassTransit.Saga;

    #endregion

    public class FakeSaga : ISaga
    {
        #region Public Properties

        public IServiceBus Bus { get; set; }

        public Guid CorrelationId { get; set; }

        public string Property { get; set; }

        #endregion
    }
}