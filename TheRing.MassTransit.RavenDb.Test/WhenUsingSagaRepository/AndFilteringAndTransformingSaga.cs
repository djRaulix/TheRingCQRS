namespace TheRing.MassTransit.RavenDb.Test.WhenUsingSagaRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using global::MassTransit.Saga;

    using NUnit.Framework;

    using TheRing.MassTransit.RavenDb.Test.Fakes;

    public class AndFilteringAndTransformingSaga : WhenUsingSagaRepository
    {
        private IEnumerable<string> filteredList;

        private const string FilteredPropertyValue = "notFilteredButTransformed";

        [Test]
        [Ignore("doesn't work with in memory embedded documentStore")]
        public void ThenFilteredListShouldBeReturned()
        {
            this.filteredList.Count().Should().Be(2);
            this.filteredList.Count(s => s == FilteredPropertyValue).Should().Be(2);
        }

        #region Overrides of SpecBase

        protected override void Because_of()
        {
            base.Because_of();
            filteredList = SagaRepository.Where(new SagaFilter<FakeSaga>(s => s.Property == FilteredPropertyValue), s => s.Property);
        }

        #endregion

        #region Overrides of WhenUsingSagaRepository

        protected override void Establish_context()
        {
            base.Establish_context();
            using (var session = SagaStore.OpenSession())
            {
                var saga = new FakeSaga() { CorrelationId = Guid.NewGuid(), Property = FilteredPropertyValue };
                session.Store(saga);
                saga = new FakeSaga() { CorrelationId = Guid.NewGuid(), Property = FilteredPropertyValue };
                session.Store(saga);
                saga = new FakeSaga() { CorrelationId = Guid.NewGuid(), Property = "filtered" };
                session.Store(saga);
                session.SaveChanges();
            }
        }

        #endregion
    }
}