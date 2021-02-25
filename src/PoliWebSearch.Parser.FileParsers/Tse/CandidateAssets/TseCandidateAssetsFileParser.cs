using CsvHelper;
using CsvHelper.Configuration;
using PoliWebSearch.Parser.Domain.FileParsing.Tse.CandidateAssets;
using PoliWebSearch.Parser.Infra.Services.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PoliWebSearch.Parser.FileParsers.Tse.CandidateAssets
{
    public class TseCandidateAssetsFileParser : ITseCandidateAssetsFileParser
    {
        private readonly ILogService logService;

        public TseCandidateAssetsFileParser(ILogService logService)
        {
            this.logService = logService;
        }

        public List<TseCandidateAssetsFileModel> ParseCandidateAssetsFile(string filePath)
        {
            List<TseCandidateAssetsFileModel> list = new List<TseCandidateAssetsFileModel>();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {
                Delimiter = ";",
                BadDataFound = null,
            };

            try {
                using var reader = new StreamReader(filePath, Encoding.GetEncoding("iso-8859-1"));
                using var csv = new CsvReader(reader, csvConfig);
                csv.Context.RegisterClassMap<TseCandidateAssetsModelMapping>();
                list = csv.GetRecords<TseCandidateAssetsFileModel>().ToList();
            }
            catch (Exception e) {
                logService.Log($"Unable to parse file: {filePath}", LogType.Parsing, LogLevel.Error);
                logService.Log(e);
            }
            return list;
        }
    }
}
