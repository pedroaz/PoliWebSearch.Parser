using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Shared.Services.Clock
{
    public interface IClockService
    {
        void ExecuteWithStopWatch(string actionName, Action action);
    }
}
