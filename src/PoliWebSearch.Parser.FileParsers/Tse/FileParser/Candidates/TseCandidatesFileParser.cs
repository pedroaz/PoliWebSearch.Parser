using PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace PoliWebSearch.Parser.Tse.FileParsers.Candidates
{
    public class TseCandidatesFileParser : ITseCandidatesFileParser
    {
        private readonly ILogService logService;
        private readonly IFileService fileSystem;

        public TseCandidatesFileParser(ILogService logService, IFileService fileSystem)
        {
            this.logService = logService;
            this.fileSystem = fileSystem;
        }

        public List<TseCandidateModel> ParseFile(string filePath)
        {
            List<TseCandidateModel> list = new List<TseCandidateModel>();

            try {
                CsvParserOptions csvParserOptions = new CsvParserOptions(false, ';');
                TseCandidateDataMapping csvMapper = new TseCandidateDataMapping();
                CsvParser<TseCandidateModel> csvParser = new CsvParser<TseCandidateModel>(csvParserOptions, csvMapper);
                return csvParser.ReadFromFile(filePath, Encoding.ASCII)
                    .Where(x => x.IsValid)
                    .Select(x => x.Result)
                    .ToList();
            }
            catch (System.Exception) {
                logService.Log($"Unable to parse file: {filePath}");
                return list;
            }

        }
    }
}
