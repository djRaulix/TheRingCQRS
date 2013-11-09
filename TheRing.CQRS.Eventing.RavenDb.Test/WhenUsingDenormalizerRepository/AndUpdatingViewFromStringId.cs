namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;

    using TheRing.RavenDb;

    #endregion

    public class AndUpdatingViewFromStringId : WhenUsingDenormalizerRepository
    {
        #region Constants

        private const string NewPropertyValue = "new value";

        #endregion

        #region Fields

        private string id;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenViewShouldBeUpdated()
        {
            FakeStringIdView view;
            using (var session = this.ReadModel.OpenSession())
            {
                view = session.LoadFromNonPrefixedStringId<FakeStringIdView>(this.id);
            }
            view.Property.Should().Be(NewPropertyValue);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();

            this.DenormalizerRepository.Update<FakeStringIdView>(this.id, v => v.Property = NewPropertyValue);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = "456";
            using (var session = this.ReadModel.OpenSession())
            {
                session.Store(new FakeStringIdView { FakeStringIdViewId = this.id, Property = "Init" });
                session.SaveChanges();
            }
        }

        #endregion
    }
}