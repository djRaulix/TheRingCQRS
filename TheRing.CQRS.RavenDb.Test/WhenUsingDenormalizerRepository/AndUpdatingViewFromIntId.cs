namespace TheRing.CQRS.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.RavenDb.Test.Fakes;

    #endregion

    public class AndUpdatingViewFromIntId : WhenUsingDenormalizerRepository
    {
        #region Constants

        private const string NewPropertyValue = "new value";

        #endregion

        #region Fields

        private int id;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenViewShouldBeUpdated()
        {
            FakeIntIdView view;
            using (var session = this.ReadModel.OpenSession())
            {
                view = session.Load<FakeIntIdView>(this.id);
            }
            view.Property.Should().Be(NewPropertyValue);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();

            this.DenormalizerRepository.Update<FakeIntIdView>(this.id, v => v.Property = NewPropertyValue);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = 456;
            using (var session = this.ReadModel.OpenSession())
            {
                session.Store(new FakeIntIdView { FakeIntIdViewId = this.id, Property = "Init" });
                session.SaveChanges();
            }
        }

        #endregion
    }
}