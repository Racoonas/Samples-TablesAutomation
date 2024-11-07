using Microsoft.Playwright;

namespace TablesAutomation.E2EFeaturedTests.Drivers
{
    public class ScenarioContext
    {
        public IBrowserContext? BrowserContext { get; set; }
        public IPage? Page { get; set; } //Also can be thought of as 'Tab'
        public Dictionary<string, object>? Properties { get; set; }
    }
}
