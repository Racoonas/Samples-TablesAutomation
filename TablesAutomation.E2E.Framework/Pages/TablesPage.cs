using Microsoft.Playwright;
using TablesAutomation.E2EFramework.PageComponents;

namespace TablesAutomation.E2EFramework.Pages
{
    public class TablesPage : AbstractPage
    {
        public override string PageAddress => ConfigurationContext.EnvironmentConfiguration.BaseUrl + "/components/table/overview";

        public TablesPage(IPage page) : base(page)
        {
            Page = page;
        }

        //Properties (Page Elements):
        public ILocator BasicTable => Page.Locator("table-basic-example table");
        public ILocator SortedTable => Page.Locator("table-sorting-example table");
        public ILocator BindingTable => Page.Locator("table-row-binding-example table");
        public ILocator ClickLog => Page.Locator("table-row-binding-example li");


        //Methods (Operations with the page):        
        public ItemsTable GetBasicTable()
        {
            return new ItemsTable(Page, BasicTable);
        }

        public ItemsTable GetSortedTable()
        {
            return new ItemsTable(Page, SortedTable);
        }

        public ItemsTable GetBindingTable()
        {
            return new ItemsTable(Page, BindingTable);
        }
    }
}
