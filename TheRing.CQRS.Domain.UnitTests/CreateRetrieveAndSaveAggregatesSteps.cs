using System;
using TechTalk.SpecFlow;

namespace TheRing.CQRS.Domain.UnitTests
{
    [Binding]
    public class CreateRetrieveAndSaveAggregatesSteps
    {
        [Given(@"a Guid")]
        public void GivenAGuid()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"i want a new aggregate")]
        public void WhenIWantANewAggregate()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"a new aggregate with the given guid should ne retturned")]
        public void ThenANewAggregateWithTheGivenGuidShouldNeRetturned()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
