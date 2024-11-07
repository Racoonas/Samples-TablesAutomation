using BoDi;
using Serilog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Microsoft.Playwright;
using TechTalk.SpecFlow;
using TablesAutomation.E2EFramework;
using TablesAutomation.E2EFramework.Utils;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace TablesAutomation.E2EFeaturedTests.Hooks
{
    [Binding]
    public class CommonHooks
    {
        private static readonly ILogger Logger = Log.ForContext<CommonHooks>();
        private static BrowserFactory? BrowserFactory;
        private static IBrowser? Browser;
        private readonly IObjectContainer _objectContainer; //Dependency Injection object container
        private IBrowserContext? BrowserContext;

        public CommonHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            LogManager.InitSeriLogs();
            Logger.Information("BeforeTestRun. Logs Initialized.");

            Logger.Information("BeforeTestRun. Initializing Browser...");
            BrowserFactory = new BrowserFactory(ConfigurationContext.ApplicationSettings.BrowserOptions);
            Browser = BrowserFactory.GetBrowser();
            Logger.Information("BeforeTestRun. Browser Initialized: {Browser} Version: {Version}. Thread: {ThreadId}",
               Browser.BrowserType.Name,
               Browser.Version,
               Environment.CurrentManagedThreadId);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (Browser != null)
            {
                Logger.Information("AfterTestRun. Disposing Browser: {Browser} Version: {Version}. Thread: {ThreadId}",
                    Browser.BrowserType.Name,
                    Browser.Version,
                    Environment.CurrentManagedThreadId);
                Browser.CloseAsync();

            }
            Logger.Information("AfterTestRun. Browser disposed.");
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            Logger.Information("BeforeScenario. {ClassName}.{TestName} Test is running in Thread: {ThreadId}",
                TestContext.CurrentContext.Test.ClassName,
                TestContext.CurrentContext.Test.MethodName,
                Environment.CurrentManagedThreadId);

            Logger.Information("BeforeScenario. Initializing BrowserContext...");
            if (BrowserFactory == null) throw new NullReferenceException("BeforeScenario. Browser Context can't be created. Browser Factory is null");
            BrowserContext = await BrowserFactory.GetBrowserContext();
            Logger.Information("BeforeScenario. BrowserContext initialized.");

            Logger.Information("BeforeScenario. Setting up default timeout to {DefaultTimeout}...",
                ConfigurationContext.ApplicationSettings.BrowserOptions.DefaultTimeout);
            BrowserContext.SetDefaultTimeout(ConfigurationContext.ApplicationSettings.BrowserOptions.DefaultTimeout);

            if (ConfigurationContext.ApplicationSettings.BrowserOptions.TracesCapturingEnabled)
            {

                Logger.Information("BeforeScenario. Starting Tracing...");

                await BrowserContext.Tracing.StartAsync(new()
                {
                    Screenshots = true,
                    Snapshots = true,
                    Name = TestContext.CurrentContext.Test.FullName,
                    Title = TestContext.CurrentContext.Test.FullName,
                    Sources = true,
                });

                Logger.Information("BeforeScenario. Tracing started");
            }

            Logger.Information("BeforeScenario. Opening new page inside BrowserContext...");
            var page = await BrowserContext.NewPageAsync();
            Logger.Information("BeforeScenario. New page opened.");

            Logger.Information("BeforeScenario. Setting up the viewport size.");
            var viewport = ConfigurationContext.ApplicationSettings.BrowserOptions.Viewport;
            await page.SetViewportSizeAsync(viewport.Width, viewport.Height);

            Logger.Information("BeforeScenario. Registering BrowserContext and Page in Dependency Injection container for shared use...");
            var scenarioContext = new Drivers.ScenarioContext
            {
                BrowserContext = BrowserContext,
                Page = page,
                Properties = new Dictionary<string, object>()
            };

            _objectContainer.RegisterInstanceAs(scenarioContext);
            Logger.Information("BeforeScenario. Objects registered.");
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            Logger.Information(
                "AfterScenario. Test '{TestName}' from Test Feature '{ClassName}'. ThreadId:  {ThreadId}",
                    TestContext.CurrentContext.Test.MethodName,
                    TestContext.CurrentContext.Test.ClassName,
                    Environment.CurrentManagedThreadId);

            var videoPaths = new List<string>();

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed &&
                ConfigurationContext.ApplicationSettings.BrowserOptions.TracesCapturingEnabled)
            {
                if (BrowserContext.Tracing != null)
                {
                    Logger.Information("AfterScenario. Saving traces");

                    var timestamp = DateTime.Now.ToString("yyyyddM-HHmmss.ffff");
                    var testName = TestContext.CurrentContext.Test.Name;
                    var scrPath = Path.Combine(ConfigurationContext.ApplicationSettings.BrowserOptions.TracesFolder,
                        $"{testName}-{timestamp}_trace.zip");

                    await BrowserContext.Tracing.StopAsync(new()
                    {
                        Path = scrPath
                    });
                    Logger.Information($"AfterScenario. Traces saved to {scrPath}");
                    TestContext.AddTestAttachment(scrPath, $"Test traces");
                }
                else
                {
                    Logger.Information("AfterScenario. Couldn't save traces, BrowserContext.Tracing is null");
                }
            }

            if (ConfigurationContext.ApplicationSettings.BrowserOptions.VideoCapturingEnabled)
            {
                Logger.Information("AfterScenario. Extracting recorded videos for '{TestName}'...",
                    TestContext.CurrentContext.Test.MethodName);
                try
                {
                    if (BrowserContext == null) throw new NullReferenceException("BrowserContext is null.");
                    videoPaths = VideosHelper.ExtractVideos(BrowserContext);
                    Logger.Information("AfterScenario. Videos extracted for '{TestName}'.",
                    TestContext.CurrentContext.Test.MethodName);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "AfterScenario. Couldn't extract recorded videos for '{TestName}'...",
                        TestContext.CurrentContext.Test.MethodName);
                }
            }

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed &&
                ConfigurationContext.ApplicationSettings.BrowserOptions.ScreenshotsEnabled)
            {
                Logger.Error("AfterScenario. The test '{TestName}' failed with error: '{ErrorText}'. Taking and saving screenshot",
                    TestContext.CurrentContext.Test.MethodName,
                    TestContext.CurrentContext.Result.Message);
                try
                {
                    if (BrowserContext == null) throw new NullReferenceException("BrowserContext is null.");
                    await TakeScreenshots(BrowserContext, TestContext.CurrentContext.Test.MethodName!);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "AfterScenario. Taking screenshot for '{TestName}' failed with error: '{ex}'",
                        TestContext.CurrentContext.Test.MethodName,
                        ex.Message);
                }
            }

            Logger.Information("AfterScenario. Disposing BrowserContext for '{TestName}'...",
                TestContext.CurrentContext.Test.MethodName);
            await DisposeBrowserContext();
            Logger.Information("AfterScenario. BrowserContext for '{TestName}' disposed.",
                TestContext.CurrentContext.Test.MethodName);

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                Logger.Information("AfterScenario. '{TestName}' test passed. Removing recorded videos...",
                    TestContext.CurrentContext.Test.MethodName);
                VideosHelper.RemoveVideos(videoPaths);
                Logger.Information("AfterScenario. Recorded videos removed.");
                return;
            };

            Logger.Information("AfterScenario. Renaming recorded videos for '{TestName}'...",
                TestContext.CurrentContext.Test.MethodName);
            foreach (var video in videoPaths)
            {
                var renamedVideo = VideosHelper.AddTestNamePrefix(video, TestContext.CurrentContext.Test.MethodName!.Normalize());

                Logger.Information("AfterScenario. Attaching recorded videos to the test context for '{TestName}'...",
                   TestContext.CurrentContext.Test.MethodName);
                TestContext.AddTestAttachment(renamedVideo, "Video attachment");
                Logger.Information("AfterScenario. Attached '{VideoFileName}' to the test context for '{TestName}'.",
                    renamedVideo,
                   TestContext.CurrentContext.Test.MethodName);
            }

            Logger.Information("AfterScenario. Finished for test '{TestName}' from TestClass '{ClassName}'",
                TestContext.CurrentContext.Test.MethodName,
                TestContext.CurrentContext.Test.ClassName);
        }

        private async Task TakeScreenshots(IBrowserContext BrowserContext, string testMethodName)
        {

            foreach (var page in BrowserContext.Pages)
            {
                var timestamp = DateTime.Now.ToString("yyyyddM-HHmmss.ffff");
                var scrPath = Path.Combine(ConfigurationContext.ApplicationSettings.BrowserOptions.ScreenshotFolder,
                    $"{testMethodName}-{timestamp}.png");
                await page.ScreenshotAsync(new()
                {
                    Path = scrPath,
                    FullPage = true,
                });

                Logger.Information("Attaching screenshot: '{scrPath}' videos to the test '{TestName}' context...",
                   scrPath,
                   TestContext.CurrentContext.Test.Name);

                TestContext.AddTestAttachment(scrPath, $"Page screenshot");
            }
        }

        private async Task DisposeBrowserContext()
        {
            if (BrowserContext != null)
            {
                await BrowserContext.CloseAsync();
            }
        }
    }
}
