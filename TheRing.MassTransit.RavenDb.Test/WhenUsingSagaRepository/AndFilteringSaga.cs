namespace TheRing.MassTransit.RavenDb.Test.WhenUsingSagaRepository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using global::MassTransit.Saga;

    using NUnit.Framework;

    using TheRing.MassTransit.RavenDb.Test.Fakes;

    #endregion

    public class AndFilteringSaga : WhenUsingSagaRepository
    {
        #region Constants

        private const string FilteredPropertyValue = "notFiltered";

        #endregion

        #region Fields

        private IEnumerable<FakeSaga> filteredList;

        #endregion

        #region Public Methods and Operators

        [Test]
        [Ignore("doesn't work with in memory embedded documentStore")]
        public void ThenFilteredListShouldBeReturned()
        {
            this.filteredList.Count().Should().Be(2);
            this.filteredList.Count(s => s.Property == FilteredPropertyValue).Should().Be(2);
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.filteredList =
                this.SagaRepository.Where(new SagaFilter<FakeSaga>(s => s.Property == FilteredPropertyValue));
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            using (var session = this.SagaStore.OpenSession())
            {
                var saga = new FakeSaga { CorrelationId = Guid.NewGuid(), Property = FilteredPropertyValue };
                session.Store(saga);
                saga = new FakeSaga { CorrelationId = Guid.NewGuid(), Property = FilteredPropertyValue };
                session.Store(saga);
                saga = new FakeSaga { CorrelationId = Guid.NewGuid(), Property = "filtered" };
                session.Store(saga);
                session.SaveChanges();
            }
        }

        #endregion
    }
}