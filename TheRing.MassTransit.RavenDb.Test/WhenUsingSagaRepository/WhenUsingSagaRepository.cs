namespace TheRing.MassTransit.RavenDb.Test.WhenUsingSagaRepository
{
    #region using

    using TheRing.CQRS.MassTransit.RavenDb;
    using TheRing.MassTransit.RavenDb.Test.Fakes;

    #endregion

    public abstract class WhenUsingSagaRepository : Specification
    {
        #region Properties

        protected SagaRepository<FakeSaga> SagaRepository { get; private set; }

        #endregion

        #region Methods

        protected override void Establish_context()
        {
            base.Establish_context();
            this.SagaRepository = new SagaRepository<FakeSaga>(this.SagaStore);
        }

        #endregion
    }
}