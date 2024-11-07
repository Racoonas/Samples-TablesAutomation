using Microsoft.Playwright;
using TechTalk.SpecFlow;
using TablesAutomation.E2EFramework.Pages;

namespace TablesAutomation.E2EFeaturedTests.Steps
{
    [Binding]
    public class TablePageSteps
    {
        private readonly Drivers.ScenarioContext _scenarioContext;
        private readonly IBrowserContext _browserContext;
        private readonly IPage _page;

        public TablePageSteps(Drivers.ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _browserContext = scenarioContext.BrowserContext ??
                throw new ArgumentException("Browser Context can't be null when instantiating Page-steps class");
            _page = scenarioContext.Page ??
                throw new ArgumentException("Page can't be null when instantiating Page-steps class");
        }

        [Given(@"I Navigate to Angular tables page")]
        [When(@"I Navigate to Angular tables page")]
        public async Task NavigateToPage()
        {
            await new Router().OpenTablesPage(_page);
        }

        [Given(@"I wait until Angular tables page fully loaded")]
        [When(@"I wait until Angular tables page fully loaded")]
        public async Task WaitUntilPageFullyLoaded()
        {
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            var tablesPage = new TablesPage(_page);
            await tablesPage.BasicTable.WaitForAsync();
            await tablesPage.SortedTable.WaitForAsync();
        }
    }
}
