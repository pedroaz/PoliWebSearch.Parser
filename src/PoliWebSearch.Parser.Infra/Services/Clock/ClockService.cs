using PoliWebSearch.Parser.Infra.Services.Log;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.Infra.Services.Clock
{
    /// <summary>
    /// Implementation of IClockService
    /// </summary>
    public class ClockService : IClockService
    {
        private readonly ILogService logService;

        public ClockService(ILogService logService)
        {
            this.logService = logService;
        }

        // <inheritdoc/>
        public void ExecuteWithStopWatch(string actionName, Action action)
        {
            logService.Log($"Starting to execute: {actionName}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            action();
            stopWatch.Stop();
            var elapsedTimeSpan = stopWatch.Elapsed;
            logService.Log($"Amount of time elasped for the parse: {elapsedTimeSpan}");
        }

        // <inheritdoc/>
        public async Task ExecuteWithStopWatchAsync(string actionName, Func<Task> asyncTask)
        {
            logService.Log($"Starting to execute: {actionName}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await asyncTask();
            stopWatch.Stop();
            var elapsedTimeSpan = stopWatch.Elapsed;
            logService.Log($"Amount of time elasped for the parse: {elapsedTimeSpan}");
        }
    }
}
