namespace TheRing.CQRS.Eventing.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.Eventing.RavenDb.Test.Fakes;

    #endregion

    public class AndUpdatingViewFromGuidId : WhenUsingDenormalizerRepository
    {
        #region Fields

        private Guid id;
        private const string NewPropertyValue = "new value";

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
            view.Property.Should().Be(NewPropertyValue);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            
            this.DenormalizerRepository.Update<FakeGuidIdView>(this.id, v => v.Property = NewPropertyValue);
        }

        #region Overrides of WhenUsingDenormalizerRepository

        protected override void Establish_context()
        {
            base.Establish_context();
            this.id = new Guid();
            using (var session = this.ReadModel.OpenSession())
            {
                session.Store(new FakeGuidIdView(){FakeGuidIdViewId = this.id, Property = "Init"});
                session.SaveChanges();
            }
        }

        #endregion

        #endregion
    }
}