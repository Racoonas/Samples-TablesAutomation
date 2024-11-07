using Microsoft.Playwright;
using System.Data;
using TechTalk.SpecFlow;
using TablesAutomation.E2EFramework.Pages;
using TablesAutomation.E2EFramework.Utils;

namespace TablesAutomation.E2EFeaturedTests.Steps
{
    [Binding]
    public class SortedTableSteps
    {
        private readonly Drivers.ScenarioContext _scenarioContext;
        private readonly IBrowserContext _browserContext;
        private readonly IPage _page;

        public SortedTableSteps(Drivers.ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _browserContext = scenarioContext.BrowserContext ??
                throw new ArgumentException("Browser Context can't be null when instantiating Page-steps class");
            _page = scenarioContext.Page ??
                throw new ArgumentException("Page can't be null when instantiating Page-steps class");
        }

        [Then(@"Sorted table is displayed")]
        public async Task SortedTableShouldBeDisplayed()
        {
            var tablesPage = new TablesPage(_page);
            await Assertions.Expect(tablesPage.SortedTable).ToBeVisibleAsync();
        }

        [Given(@"I click on '(.*)' column header in Sorted Table")]
        [When(@"I click on '(.*)' column header in Sorted Table")]
        public async Task ApplyFilterToColumn(string column)
        {
            var tablesPage = new TablesPage(_page);
            await tablesPage.GetSortedTable().ClickColumnHeader(column);
        }

        [Then(@"Rows are sorted by '(.*)' column in '(.*)' order")]
        public async Task CheckColumnSorting(string column, string order)
        {
            var tablesPage = new TablesPage(_page);

            switch (order)
            {
                case "ascending":
                case "descending":
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), $"Unkown order: {order}. Please use 'ascending' or 'descending'");
            }

            var columnValues = await tablesPage.GetSortedTable().GetColumnValues(column);

            if (column.Equals("Weight")) //Columns in Decimal format. Need parsing before checking order.
            {
                CustomCollectionAssert.AssertCollectionSorted(columnValues.Select(val => decimal.Parse(val)), order, Comparer<Decimal>.Default);
                return;
            }

            CustomCollectionAssert.AssertCollectionSorted(columnValues, order, StringComparer.OrdinalIgnoreCase);
        }

    }
}
