using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSearch.Parser.Tse.FileParsers.Candidates;
using PoliWebSerach.Parser.DB.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.Tse.Service
{
    public enum TseDataSourceType
    {
        candidatos,
        resultados
    }

    public class TseParserService : ITseParserService
    {
        private readonly ILogService logService;
        private readonly IConfiguratorService configurator;
        private readonly ITseCandidatesFileParser candidatesFileParser;
        private readonly IFileService fileService;
        private readonly IClockService clockService;
        private readonly IDatabaseService databaseService;

        public TseParserService(ILogService logService, IConfiguratorService configurator, ITseCandidatesFileParser candidatesFileParser,
            IFileService fileService, IClockService clockService, IDatabaseService databaseService)
        {
            this.logService = logService;
            this.configurator = configurator;
            this.candidatesFileParser = candidatesFileParser;
            this.fileService = fileService;
            this.clockService = clockService;
            this.databaseService = databaseService;
        }

        public async Task<int> ParseFiles(TseDataSourceType dataSource)
        {
            logService.Log("Starting to parse tse files");
            switch (dataSource) {
                case TseDataSourceType.candidatos:
                    await ParseCandidates();
                    break;
                case TseDataSourceType.resultados:
                    break;
            }



            return 0;
        }

        private async Task ParseCandidates()
        {
            string dirPath = Path.Join(configurator.AppConfig.StorageDirectory, "Tse", "Candidatos");
            if (!fileService.DirExists(dirPath)) return;
            var files = fileService.GetFilesFromDir(dirPath);
            List<TseCandidateModel> list = new List<TseCandidateModel>();
            clockService.ExecuteWithStopWatch("Parsing TSE Candidates", () => {
                foreach (var file in files) {
                    logService.Log($"Parsing File: {file}");
                    list.AddRange(candidatesFileParser.ParseFile(file));
                }
            });
            logService.Log($"Amount of records {list.Count}");

            await databaseService.AddVertices(new List<TseCandidateModel>(){ list[0] }, "person", "1");
        }
    }
}
