using CsvHelper;
using CsvHelper.Configuration;
using PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PoliWebSearch.Parser.Tse.FileParsers.Candidates
{
    public class TseCandidatesFileParser : ITseCandidatesFileParser
    {
        private readonly ILogService logService;

        public TseCandidatesFileParser(ILogService logService)
        {
            this.logService = logService;
        }

        public List<TseCandidateFileModel> ParseFile(string filePath)
        {
            List<TseCandidateFileModel> list = new List<TseCandidateFileModel>();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {
                Delimiter = ";",
                BadDataFound = null,
            };

            try {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, csvConfig)) {
                    csv.Context.RegisterClassMap<TseCandidateModelMapping>();
                    list = csv.GetRecords<TseCandidateFileModel>().ToList();
                }
            }
            catch (Exception e) {
                logService.Log($"Unable to parse file: {filePath}");
                logService.Log(e.Message);
            }
            return list;
        }
    }
}
