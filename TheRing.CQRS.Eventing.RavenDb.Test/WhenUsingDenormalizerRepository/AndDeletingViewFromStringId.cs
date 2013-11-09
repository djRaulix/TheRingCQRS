namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;
    using TheRing.RavenDb;

    #endregion

    public class AndDeletingViewFromStringId : WhenUsingDenormalizerRepository
    {
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
            view.Should().BeNull();
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            
            this.DenormalizerRepository.Delete<FakeStringIdView>(this.id);
        }

        #region Overrides of WhenUsingDenormalizerRepository

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = "77";
            using (var session = this.ReadModel.OpenSession())
            {
                session.Store(new FakeStringIdView(){FakeStringIdViewId = this.id});
                session.SaveChanges();
            }
        }

        #endregion

        #endregion
    }
}