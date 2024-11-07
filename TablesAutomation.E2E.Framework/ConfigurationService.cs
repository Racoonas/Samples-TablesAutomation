using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Serilog;
using TablesAutomation.E2EFramework.Interfaces;

namespace TablesAutomation.E2EFramework
{
    public class ConfigurationService : IConfigurationService
    {
        //For static members, Serilog Logger needs to be defined as a property.
        //See issue: https://github.com/serilog/serilog/issues/1665
        private static ILogger Logger => Log.ForContext<ConfigurationService>();
        private string _pathToConfig;

        public ConfigurationService(string pathToConfig)
        {
            Logger.Information("Creating configuration service for the file: {PathToConfig}", pathToConfig);
            if (string.IsNullOrEmpty(pathToConfig))
            {
                throw new ArgumentException("Path to config file is not valid. It's null or empty");
            }
            _pathToConfig = pathToConfig;
        }

        public T InitConfiguration<T>()
        {
            Logger.Information("Initializing configuration with file: {PathToConfig} and model: {DeserializationModel}", _pathToConfig, typeof(T));
            try
            {
                var configuration = JsonConvert.DeserializeObject<T>(
                    File.ReadAllText(_pathToConfig),
                    new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
                Logger.Information($"Configuration: {Environment.NewLine} {@configuration}");
                return configuration;
            }
            catch (JsonSerializationException ex)
            {
                throw new JsonSerializationException(ex.GetType().Name + " : " + ex.Message);
            }
        }

        public void Validate(string pathToSchema)
        {
            Logger.Information($"Validating configuration file: {_pathToConfig} with Schema: {pathToSchema}");

            if (string.IsNullOrEmpty(_pathToConfig))
            {
                throw new ArgumentException("Couldn't Validate file as the path is null or empty");
            }

            JSchema schema;

            using (var file = File.OpenText(pathToSchema))
            using (var reader = new JsonTextReader(file))
            {
                schema = JSchema.Load(reader);
            }

            var parsedJson = JObject.Parse(File.ReadAllText(_pathToConfig));
            var valid = parsedJson.IsValid(schema);

            if (!valid)
            {
                throw new ArgumentException($@"Json file: {_pathToConfig} validation failed.                                               
                                              Please check the file with following schema: {pathToSchema} ");
            }
        }

    }


}
