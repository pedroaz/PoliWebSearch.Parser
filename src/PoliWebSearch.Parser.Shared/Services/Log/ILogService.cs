using System;
using System.Runtime.CompilerServices;

namespace PoliWebSearch.Parser.Shared.Services.Log
{
    /// <summary>
    /// Class responsable for the application logs
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Initializes the logger
        /// </summary>
        void Initialize();
        /// <summary>
        /// Log a message to all sinks
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="file"></param>
        /// <param name="caller"></param>
        /// <param name="number"></param>
        void Log(string message, LogType type = LogType.None, LogLevel level = LogLevel.Information,
           [CallerFilePath] string file = "", [CallerMemberName] string caller = "", [CallerLineNumber] int number = 0);

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <param name="method"></param>
        /// <param name="number"></param>
        void Log(Exception e, LogType type = LogType.None,
            [CallerFilePath] string file = "", [CallerMemberName] string method = "", [CallerLineNumber] int number = 0);

        /// <summary>
        /// Log to Console.WriteLine. Will not pass into the log sinks
        /// </summary>
        /// <param name="message"></param>
        void LogToConsole(string message);
    }
}
