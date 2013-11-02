namespace TheRing.Test
{
    #region using

    using NUnit.Framework;

    #endregion

    [TestFixture]
    public abstract class SpecBase
    {
        #region Public Methods and Operators

        [TestFixtureSetUp]
        public virtual void MainSetup()
        {
            this.Establish_context();
            this.Because_of();
        }

        [TestFixtureTearDown]
        public virtual void MainTeardown()
        {
            this.Cleanup();
        }

        #endregion

        #region Methods

        protected virtual void Because_of()
        {
        }

        protected virtual void Cleanup()
        {
        }

        protected virtual void Establish_context()
        {
        }

        #endregion
    }
}