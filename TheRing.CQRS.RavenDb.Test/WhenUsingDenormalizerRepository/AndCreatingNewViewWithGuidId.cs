namespace TheRing.CQRS.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.RavenDb.Test.Fakes;

    #endregion

    public class AndCreatingNewViewWithGuidId : WhenUsingDenormalizerRepository
    {
        #region Fields

        private Guid id;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenViewShouldBeStored()
        {
            FakeGuidIdView view;
            using (var session = this.ReadModel.OpenSession())
            {
                view = session.Load<FakeGuidIdView>(this.id);
            }
            view.FakeGuidIdViewId.Should().Be(this.id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.id = new Guid();
            this.DenormalizerRepository.Create<FakeGuidIdView>(v => v.FakeGuidIdViewId = this.id);
        }

        #endregion
    }
}