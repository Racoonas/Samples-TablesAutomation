using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TablesAutomation.E2EFramework.Models
{
    public class AppSettingsConfig
    {

        [JsonProperty] public AppSettings AppSettings { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AppSettings
    {
        [JsonProperty] public BrowserOptions BrowserOptions { get; set; }
        [JsonProperty] public bool JavaScriptErrorLogging { get; set; }
        [JsonProperty] public UiTimeouts UiTimeouts { get; set; }        
        [JsonProperty] public FirefoxPreferences FirefoxPreferences { get; set; }
        [JsonProperty] public ChromePreferences ChromePreferences { get; set; }
    }

    public class UiTimeouts
    {
        [JsonProperty(Required = Required.Always)]
        public string PageRenderMaxTimeoutSeconds { get; set; }
    }

    public class BrowserOptions
    {
        [JsonConverter(typeof(StringEnumConverter))] public BrowserType BrowserType { get; set; }
        [JsonProperty] public bool IsHeadlessEnabled { get; set; }
        [JsonProperty] public string DownloadFolder { get; set; }
        [JsonProperty] public bool ScreenshotsEnabled { get; set; }
        [JsonProperty] public string ScreenshotFolder { get; set; }
        [JsonProperty] public bool VideoCapturingEnabled { get; set; }
        [JsonProperty] public string CapturedVideosFolder { get; set; }
        [JsonProperty] public bool TracesCapturingEnabled { get; set; }
        [JsonProperty] public string TracesFolder { get; set; }
        [JsonProperty] public Viewport Viewport { get; set; }
        [JsonProperty] public float DefaultTimeout { get; set; }
    }

    public class Viewport
    {
        [JsonProperty] public int Width { get; set; }
        [JsonProperty] public int Height { get; set; }
    }

    public class FirefoxPreferences
    {
    }

    public class ChromePreferences
    {
    }
}