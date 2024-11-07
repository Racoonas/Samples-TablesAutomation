using Newtonsoft.Json;

namespace TablesAutomation.E2EFramework.Models
{
    public class EnvSettingsConfig
    {

        [JsonProperty] public EnvConfiguration EnvConfiguration { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class EnvConfiguration
    {
        [JsonProperty] public string BaseUrl { get; set; }        
    }
   
}