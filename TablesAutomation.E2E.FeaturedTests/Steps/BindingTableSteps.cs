using Microsoft.Playwright;
using System.Data;
using TechTalk.SpecFlow;
using TablesAutomation.E2EFramework.Pages;
using NUnit.Framework.Legacy;

namespace TablesAutomation.E2EFeaturedTests.Steps
{
    [Binding]
    public class BindingTableSteps
    {
        private readonly Drivers.ScenarioContext _scenarioContext;
        private readonly IBrowserContext _browserContext;
        private readonly IPage _page;

        public BindingTableSteps(Drivers.ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _browserContext = scenarioContext.BrowserContext ??
                throw new ArgumentException("Browser Context can't be null when instantiating Page-steps class");
            _page = scenarioContext.Page ??
                throw new ArgumentException("Page can't be null when instantiating Page-steps class");
        }

        [Then(@"Binding table is displayed")]
        public async Task BindingTableShouldBeDisplayed()
        {
            var tablesPage = new TablesPage(_page);
            await Assertions.Expect(tablesPage.BindingTable).ToBeVisibleAsync();
        }

        [Given(@"I click on the following row in Binding table")]
        [When(@"I click on the following row in Binding table")]
        public async Task ClickRow(Table table)
        {
            if (table.RowCount != 1) throw new ArgumentException("Only a single row allowed in this method");

            var columnValues = table.Rows.First().Values;
            if (columnValues.Count() == 0) throw new ArgumentException("Specify at least 1 column in the row");

            var tablesPage = new TablesPage(_page);
            var row = await tablesPage.GetBindingTable().GetRow(columnValues.ToArray());

            await row.ClickAsync();
        }

        [Then(@"Click Log contains following text:")]
        public async Task ClickLogHasText(string text)
        {
            var tablesPage = new TablesPage(_page);
            var clickLog = await tablesPage.ClickLog.AllAsync();
            var texts = await Task.WhenAll(clickLog.Select(e => e.TextContentAsync()));

            CollectionAssert.Contains(texts, text);
        }
    }
}
