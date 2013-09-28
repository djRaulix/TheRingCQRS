namespace TheRing.CQRS.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.RavenDb.Test.Fakes;

    #endregion

    public class AndCreatingNewViewWithIntId : WhenUsingDenormalizerRepository
    {
        #region Fields

        private int id;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenViewShouldBeStored()
        {
            FakeIntIdView view;
            using (var session = this.ReadModel.OpenSession())
            {
                view = session.Load<FakeIntIdView>(this.id);
            }
            view.FakeIntIdViewId.Should().Be(this.id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.id = 123;
            this.DenormalizerRepository.Create<FakeIntIdView>(v => v.FakeIntIdViewId = this.id);
        }

        #endregion
    }
}