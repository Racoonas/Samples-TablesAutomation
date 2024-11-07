using Microsoft.Playwright;

namespace TablesAutomation.E2EFramework.Pages
{
    public class Router
    {
        public async Task<TablesPage> OpenTablesPage(IPage page)
        {
            var loginPage = await new TablesPage(page).GoTo();
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            return (TablesPage)loginPage;
        }
    }
}
