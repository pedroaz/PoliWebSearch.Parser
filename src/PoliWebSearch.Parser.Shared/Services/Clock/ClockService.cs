using PoliWebSearch.Parser.Shared.Services.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.Shared.Services.Clock
{
    public class ClockService : IClockService
    {
        private readonly ILogService logService;

        public ClockService(ILogService logService)
        {
            this.logService = logService;
        }

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
