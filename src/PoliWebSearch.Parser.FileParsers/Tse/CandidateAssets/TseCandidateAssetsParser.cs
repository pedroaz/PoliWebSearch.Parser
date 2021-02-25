using PoliWebSearch.Parser.Domain.FileParsing.Tse.CandidateAssets;
using PoliWebSearch.Parser.Infra.Configurator;
using PoliWebSearch.Parser.Infra.Services.Clock;
using PoliWebSearch.Parser.Infra.Services.IO;
using PoliWebSearch.Parser.Infra.Services.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.CandidateAssets
{
    public class TseCandidateAssetsParser : ITseCandidateAssetsParser
    {
        private readonly ITseCandidateAssetsFileParser fileParser;
        private readonly IFileService fileService;
        private readonly IConfiguratorService configurator;
        private readonly IClockService clockService;
        private readonly ILogService logService;

        public TseCandidateAssetsParser(ITseCandidateAssetsFileParser fileParser, IFileService fileService, IConfiguratorService configuratorService,
            IClockService clockService, ILogService logService)
        {
            this.fileParser = fileParser;
            this.fileService = fileService;
            this.configurator = configuratorService;
            this.clockService = clockService;
            this.logService = logService;
        }

        public async Task ParseCandidateAssets(int rowLimit)
        {
            string dirPath = Path.Join(configurator.AppConfig.StorageDirectory, "Tse", "BensCandidatos");
            if (!fileService.DirExists(dirPath)) return;
            var files = fileService.GetFilesFromDir(dirPath);
            List<TseCandidateAssetsFileModel> list = new List<TseCandidateAssetsFileModel>();
            clockService.ExecuteWithStopWatch("Parsing TSE Candidates", () => {
                foreach (var file in files) {
                    logService.Log($"Parsing File: {file}");
                    list.AddRange(fileParser.ParseCandidateAssetsFile(file));
                }
            });

            // Remove rows if needed
            if (rowLimit > 0) {
                list = list.Take(rowLimit).ToList();
            }

            await InsertDataIntoDatabase(list);
        }

        private async Task InsertDataIntoDatabase(List<TseCandidateAssetsFileModel> list)
        {
            throw new NotImplementedException();
        }
    }
}
