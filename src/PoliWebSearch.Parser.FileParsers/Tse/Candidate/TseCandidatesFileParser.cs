using CsvHelper;
using CsvHelper.Configuration;
using PoliWebSearch.Parser.Domain.FileParsing.Tse.Candidate;
using PoliWebSearch.Parser.Infra.Services.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PoliWebSearch.Parser.FileParsers.Tse.Candidate
{
    /// <summary>
    /// Implementation of ITseCandidatesFileParser
    /// </summary>
    public class TseCandidatesFileParser : ITseCandidatesFileParser
    {
        private readonly ILogService logService;

        public TseCandidatesFileParser(ILogService logService)
        {
            this.logService = logService;
        }

        // <inheritdoc/>
        public List<TseCandidateFileModel> ParseCandidateFile(string filePath)
        {
            List<TseCandidateFileModel> list = new List<TseCandidateFileModel>();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {
                Delimiter = ";",
                BadDataFound = null,
            };

            try {
                using (var reader = new StreamReader(filePath, Encoding.GetEncoding("iso-8859-1")))
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
