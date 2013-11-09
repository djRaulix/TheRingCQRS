namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;

    #endregion

    public class AndDeletingViewFromGuidId : WhenUsingDenormalizerRepository
    {
        #region Fields

        private Guid id;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenViewShouldBeUpdated()
        {
            FakeGuidIdView view;
            using (var session = this.ReadModel.OpenSession())
            {
                view = session.Load<FakeGuidIdView>(this.id);
            }

            view.Should().BeNull();
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();

            this.DenormalizerRepository.Delete<FakeGuidIdView>(this.id);
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = new Guid();
            using (var session = this.ReadModel.OpenSession())
            {
                session.Store(new FakeGuidIdView { FakeGuidIdViewId = this.id });
                session.SaveChanges();
            }
        }

        #endregion
    }
}