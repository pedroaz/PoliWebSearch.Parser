using PoliWebSearch.Parser.Shared.Configurator;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace PoliWebSearch.Parser.Shared.Services.Log
{
    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }

    public enum LogType
    {
        None,
        Initialization,
        Loop,
        Admin,
        Database
    }

    public class LogService : ILogService
    {
        private Logger logger;
        private readonly IConfiguratorService configuratorService;
        private string logFile = "";

        public LogService(IConfiguratorService configuratorService)
        {
            this.configuratorService = configuratorService;
        }

        public void Initialize()
        {
            SetupLogFolder();

            logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFile)
                .CreateLogger();
        }

        private void SetupLogFolder()
        {
            var logDirectory = configuratorService.AppConfig.LogDirectory;
            if (!Directory.Exists(logDirectory)) {
                Directory.CreateDirectory(logDirectory);
            }
            int logFileCount = Directory.GetFiles(logDirectory).Length + 1;
            logFile = Path.Combine(logDirectory, $"{logFileCount}_Parser_Log.txt");
        }

        public void Log(string message, LogType type = LogType.None, LogLevel level = LogLevel.Information,
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

        public void Log(Exception e, LogType type = LogType.None, [CallerFilePath] string file = "", [CallerMemberName] string method = "", [CallerLineNumber] int number = 0)
        {
            Log(e.Message, type: type, level: LogLevel.Error);
        }

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        private string CreateLogLine(string message, string fileName, string caller, int number, LogType type)
        {
            return $"[{fileName}@{number}->{caller}]{GetLogPrefix(type)} {message}";
        }

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

        public void LogToConsole(string message)
        {
            Console.Write(message);
        }
    }
}
