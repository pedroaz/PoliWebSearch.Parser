using MoreLinq.Extensions;
using PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates;
using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Models.Edges;
using PoliWebSearch.Parser.Shared.Models.Person;
using PoliWebSearch.Parser.Shared.Models.PoliticalParty;
using PoliWebSearch.Parser.Shared.Models.Tse;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSerach.Parser.DB.Operator;
using PoliWebSerach.Parser.DB.Services.Admin;
using PoliWebSerach.Parser.DB.Services.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.Service
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
        private readonly IAdminService adminService;

        public TseParserService(ILogService logService, IConfiguratorService configurator, ITseCandidatesFileParser candidatesFileParser,
            IFileService fileService, IClockService clockService, IDatabaseService databaseService, IAdminService adminService)
        {
            this.logService = logService;
            this.configurator = configurator;
            this.candidatesFileParser = candidatesFileParser;
            this.fileService = fileService;
            this.clockService = clockService;
            this.databaseService = databaseService;
            this.adminService = adminService;
        }

        public async Task<int> ParseFiles(TseDataSourceType dataSource, int rowLimit, bool dropFirst)
        {
            logService.Log("Starting to parse tse files");
            if (dropFirst) {
                logService.Log("Droping the database before inserting");
                await adminService.DropDatabase();
            }
            switch (dataSource) {
                case TseDataSourceType.candidatos:
                    await ParseCandidates(rowLimit);
                    break;
                case TseDataSourceType.resultados:
                    break;
            }



            return 0;
        }

        private async Task ParseCandidates(int rowLimit)
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

            if (rowLimit > 0) {
                list = list.Take(rowLimit).ToList();
            }


            logService.Log($"Amount of records {list.Count}");

            await InserPoliticalPartyVertices(list);
            await InserPeopleVertices(list);
            await InsertBelongsToPartyEdges(list);

        }

        private async Task InserPeopleVertices(List<TseCandidateFileModel> list)
        {
            var peopleList = list.Select(x =>
                            new PersonVertice() {
                                CandidateName = x.CandidateName,
                                Cpf = x.Cpf
                            }
                        ).ToList();

            await databaseService.AddVertices(peopleList, "person", "1");
        }

        private async Task InserPoliticalPartyVertices(List<TseCandidateFileModel> list)
        {
            var partyList = list.Select(x =>
                            new PoliticalPartyVertice() {
                                PoliticalPartyAbbreviation = x.PolitcPartyAbbreviation,
                                PoliticalPartyName = x.PolitcPartyName
                            }
                        ).ToList();

            partyList = partyList.DistinctBy(x => x.PoliticalPartyAbbreviation).ToList();

            await databaseService.AddVertices(partyList, "political_party", "1");
        }

        private async Task InsertBelongsToPartyEdges(List<TseCandidateFileModel> list)
        {

            List<CandidateBelongsToPartyEdge> edgeProperties = new List<CandidateBelongsToPartyEdge>();
            List<VerticeFilter> fromFilters = new List<VerticeFilter>();
            List<VerticeFilter> toFilters = new List<VerticeFilter>();

            foreach (var item in list) {

                edgeProperties.Add(new CandidateBelongsToPartyEdge());
                fromFilters.Add(new VerticeFilter("Cpf", item.Cpf));
                toFilters.Add(new VerticeFilter("PolitcPartyAbbreviation", item.PolitcPartyAbbreviation));
            }

            await databaseService.AddEdges(edgeProperties, "belongs_to_party", fromFilters, toFilters);
        }

    }
}
