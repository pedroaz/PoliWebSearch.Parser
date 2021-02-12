using Newtonsoft.Json;
using PoliWebSearch.Parser.Infra.Configurator;
using PoliWebSearch.Parser.Infra.Services.IO;
using PoliWebSearch.Parser.Infra.Services.Log;
using System.IO;

namespace PoliWebSearch.Parser.Infra.Services.Result
{
    public class ResultService : IResultService
    {
        private readonly ILogService logService;
        private readonly IConfiguratorService configuratorService;
        private readonly IFileService fileService;
        private ResultData Data { get; set; } = new ResultData();

        public ResultService(ILogService logService, IConfiguratorService configuratorService, IFileService fileService)
        {
            this.logService = logService;
            this.configuratorService = configuratorService;
            this.fileService = fileService;
        }

        public void AddDatabaseCount(long value)
        {
            Data.GraphCounts.Add(value);
            WriteResultFile();
        }

        public void WriteResultFile()
        {
            var path = Path.Combine(configuratorService.AppConfig.ResultsDirectory, $"{configuratorService.AppConfig.ExecutionId}_Parser_Result.json");
            var jsonString = JsonConvert.SerializeObject(Data, Formatting.Indented);
            fileService.WriteAllText(jsonString, path);
            logService.Log("Result File Updated", LogType.Result);
        }
    }
}
