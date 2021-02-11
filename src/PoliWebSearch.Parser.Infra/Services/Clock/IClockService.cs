using System;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.Infra.Services.Clock
{
    /// <summary>
    /// Implementation of tasks related to time
    /// </summary>
    public interface IClockService
    {
        /// <summary>
        /// Executs certain action and records how long it took
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        void ExecuteWithStopWatch(string actionName, Action action);

        /// <summary>
        /// Executs certain async action and records how long it took
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        Task ExecuteWithStopWatchAsync(string actionName, Func<Task> asyncTask);
    }
}
