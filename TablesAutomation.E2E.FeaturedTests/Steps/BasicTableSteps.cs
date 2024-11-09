using Microsoft.Playwright;
using System.Data;
using TechTalk.SpecFlow;
using TablesAutomation.E2EFeaturedTests.SpecflowTableTypes;
using TablesAutomation.E2EFramework.Pages;
using TablesAutomation.E2EFramework.Utils;
using NUnit.Framework;

namespace TablesAutomation.E2EFeaturedTests.Steps
{
    [Binding]
    public class BasicTableSteps
    {
        private readonly Drivers.ScenarioContext _scenarioContext;
        private readonly IBrowserContext _browserContext;
        private readonly IPage _page;

        public BasicTableSteps(Drivers.ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _browserContext = scenarioContext.BrowserContext ??
                throw new ArgumentException("Browser Context can't be null when instantiating Page-steps class");
            _page = scenarioContext.Page ??
                throw new ArgumentException("Page can't be null when instantiating Page-steps class");
        }
                
        [Then(@"Basic table is displayed")]
        public async Task BasicTableShouldBeDisplayed()
        {
            var tablesPage = new TablesPage(_page);
            await Assertions.Expect(tablesPage.BasicTable).ToBeVisibleAsync();
        }


        [Then(@"Basic table has '(.*)' rows in Manage Builders page")]
        public async Task CountRowsInTable(int rowcount)
        {
            var tablesPage = new TablesPage(_page);
            await Assertions.Expect(tablesPage.BasicTable.Locator("//tbody//tr")).ToHaveCountAsync(rowcount);
        }

        [Then(@"Basic table contains following rows:")]
        public async Task TableHasRows(Table expectedTable)
        {
            if (expectedTable.RowCount == 0) throw new ArgumentException("At least one row should be specified");

            var tablesPage = new TablesPage(_page);
            await tablesPage.BasicTable.WaitForAsync();

            //Getting actual data table from the page:
            var actualDataTable = await tablesPage.GetBasicTable().AsDataTable();

            //Converting expected table into DataTable:
            var expectedDataTable = expectedTable.ToDataTable();

            //Comparing 2 tables:
            actualDataTable.ShouldContainRows(expectedDataTable);
        }

        [Then(@"Basic table has following columns headers:")]
        public async Task TableHasHeaders(Table table)
        {
            var tablesPage = new TablesPage(_page);
            var headers = await tablesPage.GetBasicTable().GetHeaders();

            foreach (var row in table.Rows)
            {
                var headerFound = headers.Where(h => h.Equals(row["Header"])).Any();
                Assert.That(headerFound, Is.True, $"Following header wasn't found: {row["Header"]}");
            }
        }

        [Then(@"Row number '(.*)' in Basic Table contains following values:")]        
        public async Task RowHasValues(int index, Table expectedTable)
        {
            if (expectedTable.RowCount != 1) throw new ArgumentException("Only a single row allowed in this method");

            var columnValues = expectedTable.Rows.First().Values;
            if (columnValues.Count() == 0) throw new ArgumentException("Specify at least 1 column in the row");

            var tablesPage = new TablesPage(_page);
            
            //Creating actual data table from the values in the page:
            var actualDataTable = new DataTable();

            var headers = await tablesPage.GetBasicTable().GetHeaders();
            var rowValues = await tablesPage.GetBasicTable().GetRowValues(index - 1); //since .Nth starts from 0.

            foreach (var header in headers)
            {
                actualDataTable.Columns.Add(header);
            }

            actualDataTable.Rows.Add(rowValues.ToArray());            

            //Converting expected table into DataTable:
            var expectedDataTable = expectedTable.ToDataTable();

            //Finally comparing 2 tables:
            actualDataTable.ShouldContainRows(expectedDataTable);
        }
    }
}
