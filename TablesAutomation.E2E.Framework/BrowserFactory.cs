using Serilog;
using Microsoft.Playwright;
using System.Globalization;
using TablesAutomation.E2EFramework.Models;

namespace TablesAutomation.E2EFramework
{
    public class BrowserFactory : IDisposable
    {
        private static readonly ILogger Logger = Log.ForContext<BrowserFactory>();

        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _browserContext;
        private BrowserOptions _browserOptions;

        public BrowserFactory(BrowserOptions browserOptions)
        {
            _browserOptions = browserOptions;
        }

        /// <summary>
        /// Returns the browser based on the Configuration, provided in class constructor
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public IBrowser GetBrowser()
        {
            _playwright = Playwright.CreateAsync().Result;

            BrowserTypeLaunchOptions options = new BrowserTypeLaunchOptions
            {
                Headless = _browserOptions.IsHeadlessEnabled,
                DownloadsPath = _browserOptions.DownloadFolder
            };

            switch (_browserOptions.BrowserType)
            {
                case Models.BrowserType.Firefox:
                    options.Args = new string[] { "--kiosk" };
                    _browser = _playwright.Firefox.LaunchAsync(options).Result;
                    break;
                case Models.BrowserType.Chrome:
                    options.Args = new string[] {
                        "--start-maximized",
                        "--disable-popup-blocking",
                        "--disable-web-security",
                        "--profile-directory=Default"
                    };
                    _browser = _playwright.Chromium.LaunchAsync(options).Result;
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture,
                        "Browser {0} is not supported", _browserOptions.BrowserType));
            }

            return _browser;
        }

        /// <summary>
        /// Once Browser is instantiated, call this method to get BrowserContext with related settings
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<IBrowserContext> GetBrowserContext()
        {
            if (_browser == null)
            {
                throw new Exception("Browser is null. Can't create context. Please instantiate browser first");
            }

            var browserContextOptions = new BrowserNewContextOptions { ViewportSize = ViewportSize.NoViewport };

            Logger.Information($"Browser Context. {(_browserOptions.VideoCapturingEnabled ? "Enabling video capturing..." : "Video capturing is disabled")}");

            if (_browserOptions.VideoCapturingEnabled)
            {
                browserContextOptions.RecordVideoDir = _browserOptions.CapturedVideosFolder;
                browserContextOptions.RecordVideoSize = new RecordVideoSize()
                {
                    Width = ConfigurationContext.ApplicationSettings.BrowserOptions.Viewport.Width,
                    Height = ConfigurationContext.ApplicationSettings.BrowserOptions.Viewport.Height
                };

                Logger.Information("Browser Context. Video capturing is enabled. Size: {Width}:{Height}",
                    ConfigurationContext.ApplicationSettings.BrowserOptions.Viewport.Width,
                    ConfigurationContext.ApplicationSettings.BrowserOptions.Viewport.Height);
            }

            return await _browser.NewContextAsync(browserContextOptions);
        }

        /// <summary>
        /// Disposes browser and context, once object is not in use
        /// </summary>
        public void Dispose()
        {
            if (_browserContext != null) _browserContext.CloseAsync();
            if (_browser != null) _browser.CloseAsync();
            _playwright?.Dispose();
        }
    }
}
