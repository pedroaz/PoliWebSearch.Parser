namespace PoliWebSearch.Parser.Shared.Services.Log
{
    public interface ILogService
    {
        void Initialize();
        void Log(string message);
        void LogToConsole(string message);
    }
}
