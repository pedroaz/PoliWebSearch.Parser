using System;
using System.Runtime.CompilerServices;

namespace PoliWebSearch.Parser.Shared.Services.Log
{
    public interface ILogService
    {
        void Initialize();
        void Log(string message, LogType type = LogType.None, LogLevel level = LogLevel.Information,
           [CallerFilePath] string file = "", [CallerMemberName] string caller = "", [CallerLineNumber] int number = 0);
        void Log(Exception e, LogType type = LogType.None,
            [CallerFilePath] string file = "", [CallerMemberName] string method = "", [CallerLineNumber] int number = 0);
        void LogToConsole(string message);
    }
}
