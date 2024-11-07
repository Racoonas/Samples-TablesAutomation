using Serilog;

namespace TablesAutomation.E2EFramework.Utils
{
    public class Wait
    {
        private static readonly ILogger Logger = Log.ForContext<Wait>();

        public async Task While(Func<bool> condition, TimeSpan frequency, TimeSpan timeout)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });
            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        public async Task Until(Func<bool> condition, TimeSpan frequency, TimeSpan timeout)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
                Logger.Information("Condition not met. Repeating operation");
            });
            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }
    }
}
