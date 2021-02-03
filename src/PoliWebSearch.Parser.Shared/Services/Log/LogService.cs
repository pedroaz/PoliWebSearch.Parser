using Serilog;
using Serilog.Core;
using System;

namespace PoliWebSearch.Parser.Shared.Services.Log
{
    public class LogService : ILogService
    {
        private Logger logger;

        public void Initialize()
        {
            logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public void Log(string message)
        {
            logger.Information(message);
        }

        public void LogToConsole(string message)
        {
            Console.Write(message);
        }
    }
}
