using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Models.Tse;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSearch.Parser.Tse.FileParsers.Candidates;
using PoliWebSerach.Parser.DB.Operator;
using PoliWebSerach.Parser.DB.Services;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;

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
            List<TseCandidateFileModel> list = new List<TseCandidateFileModel>();
            clockService.ExecuteWithStopWatch("Parsing TSE Candidates", () => {
                foreach (var file in files) {
                    logService.Log($"Parsing File: {file}");
                    list.AddRange(candidatesFileParser.ParseFile(file));
                }
            });
            logService.Log($"Amount of records {list.Count}");

            await databaseService.ExecuteCustomQuery("g.V().drop()");
            await InserPoliticalPartyVertices(list);
            await InserPeopleVertices(list);
            await InsertBelongsToPartyEdges(list);

        }

        private async Task InserPeopleVertices(List<TseCandidateFileModel> list)
        {
            var peopleList = list.Select(x =>
                            new TseCandidatePersonVertice() {
                                CandidateName = x.CandidateName,
                                Cpf = x.Cpf
                            }
                        ).ToList();

            await databaseService.AddVertices(peopleList, "person", "1");
        }

        private async Task InserPoliticalPartyVertices(List<TseCandidateFileModel> list)
        {
            var partyList = list.Select(x =>
                            new TseCandidatePoliticalPartyVertice() {
                                PolitcPartyAbbreviation = x.PolitcPartyAbbreviation,
                                PolitcPartyName = x.PolitcPartyName
                            }
                        ).ToList();

            partyList = partyList.DistinctBy(x => x.PolitcPartyAbbreviation).ToList();

            await databaseService.AddVertices(partyList, "political_party", "1");
        }

        private async Task InsertBelongsToPartyEdges(List<TseCandidateFileModel> list)
        {

            List<TseCandidateBelongsToPartyEdge> edgeProperties = new List<TseCandidateBelongsToPartyEdge>();
            List<VerticeFilter> fromFilters = new List<VerticeFilter>();
            List<VerticeFilter> toFilters = new List<VerticeFilter>();

            foreach (var item in list) {

                edgeProperties.Add(new TseCandidateBelongsToPartyEdge());
                fromFilters.Add(new VerticeFilter("Cpf", item.Cpf));
                toFilters.Add(new VerticeFilter("PolitcPartyAbbreviation", item.PolitcPartyAbbreviation));
            }

            await databaseService.AddEdges(edgeProperties, "belongs_to_party", fromFilters, toFilters);
        }

    }
}
