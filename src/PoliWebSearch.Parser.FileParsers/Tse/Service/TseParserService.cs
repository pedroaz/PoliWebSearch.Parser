using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSearch.Parser.Tse.FileParsers.Candidates;
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

        public TseParserService(ILogService logService, IConfiguratorService configurator, ITseCandidatesFileParser candidatesFileParser,
            IFileService fileService, IClockService clockService)
        {
            this.logService = logService;
            this.configurator = configurator;
            this.candidatesFileParser = candidatesFileParser;
            this.fileService = fileService;
            this.clockService = clockService;
        }

        public async Task<int> ParseFiles(TseDataSourceType dataSource)
        {
            logService.Log("Starting to parse tse files");
            switch (dataSource) {
                case TseDataSourceType.candidatos:
                    ParseCandidates();
                    break;
                case TseDataSourceType.resultados:
                    break;
            }



            return 0;
        }

        private void ParseCandidates()
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
        }
    }
}
