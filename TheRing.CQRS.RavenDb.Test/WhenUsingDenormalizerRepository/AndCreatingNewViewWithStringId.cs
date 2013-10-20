namespace TheRing.CQRS.RavenDb.Test.WhenUsingDenormalizerRepository
{
    #region using

    using System;

    using FluentAssertions;

    using NUnit.Framework;

    using TheRing.CQRS.RavenDb.Test.Fakes;
    using TheRing.RavenDb;

    #endregion

    public class AndCreatingNewViewWithStringId : WhenUsingDenormalizerRepository
    {
        #region Fields

        private string id;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenViewShouldBeStored()
        {
            FakeStringIdView view;
            using (var session = this.ReadModel.OpenSession())
            {
                view = session.LoadFromNonPrefixedStringId<FakeStringIdView>(this.id);
            }
            view.FakeStringIdViewId.Should().Be(this.id);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.id = "123";
            this.DenormalizerRepository.Create<FakeStringIdView>(v => v.FakeStringIdViewId = this.id);
        }

        #endregion
    }
}