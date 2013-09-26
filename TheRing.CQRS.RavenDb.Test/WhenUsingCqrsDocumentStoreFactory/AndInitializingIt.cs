namespace TheRing.CQRS.RavenDb.Test.WhenUsingCqrsDocumentStoreFactory
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.RavenDb.Test.Fakes;

    #endregion

    public class AndInitializingIt : WhenUsingCqrsDocumentStoreFactory
    {
        #region Public Methods and Operators

        [Test]
        public void ThenCqrsDocumentStoreInitializerShouldHaveFactoryInItsContext()
        {
            FakeCqrsDocumentStoreFactoryInitializer.DocumentStoreFactory.Should().NotBeNull();
            FakeCqrsDocumentStoreFactoryInitializer.DocumentStoreFactory.Should()
                .BeSameAs(this.CqrsDocumentStoreFactory);
        }

        [Test]
        public void ThenCqrsDocumentStoreInitializerShouldBeRun()
        {
            FakeCqrsDocumentStoreFactoryInitializer.Initialized.Should().BeTrue();
        }

        [Test]
        public void ThenStoresShouldBeInitialized()
        {
            this.CqrsDocumentStoreFactory.EventStore.Should().NotBeNull();
            this.CqrsDocumentStoreFactory.ReadModel.Should().NotBeNull();
            this.CqrsDocumentStoreFactory.SagaStore.Should().NotBeNull();
        }

        #endregion
    }
}