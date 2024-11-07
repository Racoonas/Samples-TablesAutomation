using TablesAutomation.E2EFramework.Models;

namespace TablesAutomation.E2EFramework;

public class ConfigurationContext
{
    //For static members, Serilog Logger needs to be a defined as a property.
    //See issue: https://github.com/serilog/serilog/issues/1665    
    private static AppSettings _appSettings;
    private static EnvConfiguration _environmentConfiguration;

    /// <summary>
    /// Deserializes and validates envsettings.json file. Lazy. Returs settings object if it already exists.
    /// </summary>
    public static EnvConfiguration EnvironmentConfiguration
    {
        get
        {
            if (_environmentConfiguration == null)
            {
                var configSvc = new ConfigurationService("envsettings.json");
                configSvc.Validate(@"Schema/envsettings-schema.json");
                _environmentConfiguration = configSvc.InitConfiguration<EnvSettingsConfig>().EnvConfiguration;
            }
            return _environmentConfiguration;
        }
    }

    /// <summary>
    /// Deserializes and validates appsettings.json file. Lazy. Returs settings object if it already exists.
    /// </summary>
    public static AppSettings ApplicationSettings
    {
        get
        {
            if (_appSettings == null)
            {
                var configSvc = new ConfigurationService("appsettings.json");
                configSvc.Validate(@"schema/appsettings-schema.json");
                _appSettings = configSvc.InitConfiguration<AppSettingsConfig>().AppSettings;
            }
            return _appSettings;
        }
    }
}