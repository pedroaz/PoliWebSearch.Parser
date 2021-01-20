using PoliWebSearch.Parser.Shared.Services.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
    }
}
