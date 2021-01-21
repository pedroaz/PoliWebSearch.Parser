using System;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.Shared.Services.Clock
{
    public interface IClockService
    {
        void ExecuteWithStopWatch(string actionName, Action action);
        Task ExecuteWithStopWatchAsync(string actionName, Func<Task> asyncTask);
    }
}
