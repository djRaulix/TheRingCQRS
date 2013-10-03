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

    public class AndFindingSaga : WhenUsingSagaRepository
    {
        #region Constants

        private const string FilteredPropertyValue = "toFind";

        #endregion

        #region Fields

        private readonly List<FakeSaga> sagaList = new List<FakeSaga>();
        private IEnumerable<Guid> filteredList;

        #endregion

        #region Public Methods and Operators

        [Test]
        [Ignore("doesn't work with in memory embedded documentStore")]
        public void ThenIdListShouldBeReturned()
        {
            this.filteredList.Count().Should().Be(2);
            foreach (var id in this.filteredList)
            {
                this.sagaList.Single(s => s.CorrelationId == id).Property.Should().Be(FilteredPropertyValue);
            }
        }

        #endregion

        #region Methods

        protected override void Because_of()
        {
            base.Because_of();
            this.filteredList =
                this.SagaRepository.Find(new SagaFilter<FakeSaga>(s => s.Property == FilteredPropertyValue));
        }

        protected override void Establish_context()
        {
            base.Establish_context();
            using (var session = this.SagaStore.OpenSession())
            {
                var saga = new FakeSaga { CorrelationId = Guid.NewGuid(), Property = FilteredPropertyValue };
                session.Store(saga);
                this.sagaList.Add(saga);
                saga = new FakeSaga { CorrelationId = Guid.NewGuid(), Property = FilteredPropertyValue };
                session.Store(saga);
                this.sagaList.Add(saga);
                saga = new FakeSaga { CorrelationId = Guid.NewGuid(), Property = "filtered" };
                session.Store(saga);
                this.sagaList.Add(saga);
                session.SaveChanges();
            }
        }

        #endregion
    }
}