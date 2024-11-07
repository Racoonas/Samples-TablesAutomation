using Microsoft.Playwright;

namespace TablesAutomation.E2EFramework.Pages
{
    public abstract class AbstractPage
    {
        protected IPage Page;

        public abstract string PageAddress { get; }

        public AbstractPage(IPage page)
        {
            Page = page;
        }

        public async Task<AbstractPage> GoTo()
        {
            if (string.IsNullOrEmpty(PageAddress))
            {
                throw new InvalidOperationException("Can't navigate to the page. Page Address wasn't specified");
            }

            await Page.GotoAsync(PageAddress, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            return this;
        }
    };

}
