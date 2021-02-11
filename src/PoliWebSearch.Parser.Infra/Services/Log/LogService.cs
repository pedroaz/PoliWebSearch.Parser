using PoliWebSearch.Parser.Infra.Configurator;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace PoliWebSearch.Parser.Infra.Services.Log
{
    /// <summary>
    /// Log level (Information, Warning or Error)
    /// </summary>
    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }

    /// <summary>
    /// Type of the log. Can be used to better filter the logs
    /// </summary>
    public enum LogType
    {
        None,
        Initialization,
        Loop,
        Admin,
        Database
    }

    /// <summary>
    /// Implmenetation of ILogService
    /// </summary>
    public class LogService : ILogService
    {
        private Logger logger;
        private readonly IConfiguratorService configuratorService;

        public LogService(IConfiguratorService configuratorService)
        {
            this.configuratorService = configuratorService;
        }

        // <inheritdoc/>
        public void Initialize()
        {
            logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(GetLogFile())
                .CreateLogger();
        }

        /// <summary>
        /// Determins the file where the file sink will log
        /// </summary>
        private string GetLogFile()
        {
            var logDirectory = configuratorService.AppConfig.LogDirectory;
            if (!Directory.Exists(logDirectory)) {
                Directory.CreateDirectory(logDirectory);
            }
            int logFileCount = Directory.GetFiles(logDirectory).Length + 1;
            return Path.Combine(logDirectory, $"{logFileCount}_Parser_Log.txt");
        }

        // <inheritdoc/>
        public void Log(string message, LogType type = LogType.None, LogLevel level = LogLevel.Information)
        {
            InternalLog(message, type, level);
        }

        // <inheritdoc/>
        public void Log(Exception e, LogType type = LogType.None)
        {
            Log(e.Message, type: type, level: LogLevel.Error);
        }

        // <inheritdoc/>
        private void InternalLog(string message, LogType type = LogType.None, LogLevel level = LogLevel.Information,
            [CallerFilePath] string file = "", [CallerMemberName] string method = "", [CallerLineNumber] int number = 0)
        {
            string line = CreateLogLine(message, Path.GetFileName(file), method, number, type);

            switch (level) {
                case LogLevel.Information:
                    logger.Information(line);
                    break;
                case LogLevel.Warning:
                    logger.Warning(line);
                    break;
                case LogLevel.Error:
                    logger.Error(line);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logs into the console. Static so it can be used by the application before the logger is initialized.
        /// If the logger is already initialized we should use LogToConsole instead
        /// </summary>
        /// <param name="message"></param>
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Create a log line using the given parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        /// <param name="caller"></param>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string CreateLogLine(string message, string fileName, string caller, int number, LogType type)
        {
            return $"[{fileName}@{number}->{caller}]{GetLogPrefix(type)} {message}";
        }

        /// <summary>
        /// Get the log prefix whcih will antecipate the line
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetLogPrefix(LogType type)
        {
            switch (type) {
                case LogType.Initialization:
                case LogType.Loop:
                case LogType.Admin:
                case LogType.Database:
                    return $"[{type}]";
                case LogType.None:
                default:
                    return "";
            }
        }

        // <inheritdoc/>
        public void LogToConsole(string message)
        {
            Console.Write(message);
        }

        
    }
}
