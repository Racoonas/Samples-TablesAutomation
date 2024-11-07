using Serilog;
using Serilog.Events;

namespace TablesAutomation.E2EFramework
{
    public class LogManager
    {
        private static readonly ILogger Logger = Log.ForContext<LogManager>();

        public static void InitSeriLogs()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u4}] [{SourceContext:l}]: {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("TestResults\\log-.txt",
                    restrictedToMinimumLevel: LogEventLevel.Verbose,
                    rollingInterval: RollingInterval.Hour,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u4}] [{SourceContext:l}]: {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Logger.Information("The logger was initiated successfully");
        }
    }
}