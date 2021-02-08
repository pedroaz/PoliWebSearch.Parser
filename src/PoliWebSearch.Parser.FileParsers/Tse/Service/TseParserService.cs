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
            await InserPeopleVertices(list);
            await InserPoliticalPartyVertices(list);
            await RemoveTseLabels();
            await InsertTseBelongsToPartyEdges(list);
        }

        private async Task InserPeopleVertices(List<TseCandidateFileModel> list)
        {
            logService.Log("Inserting People Vertices");
            var peopleList = list.Select(x =>
                            new PersonVertice() {
                                Names = new List<string>(){ x.CandidateName, x.SocialCandidateName, x.CandidateNameOnCedule },
                                Cpf = x.Cpf
                            }
                        ).ToList();

            await databaseService.AddVertices(peopleList, "person", "1", "Cpf");
        }

        private async Task InserPoliticalPartyVertices(List<TseCandidateFileModel> list)
        {
            logService.Log("Inserting Political Party Vertices");
            var partyList = list.Select(x =>
                            new PoliticalPartyVertice() {
                                PoliticalPartyAbbreviation = x.PolitcPartyAbbreviation,
                                PoliticalPartyName = x.PolitcPartyName
                            }
                        ).ToList();

            partyList = partyList.DistinctBy(x => x.PoliticalPartyAbbreviation).ToList();

            await databaseService.AddVertices(partyList, "political_party", "1", "PoliticalPartyAbbreviation");
        }


        private async Task RemoveTseLabels()
        {
            logService.Log($"Removing {TseCandidateBelongsToPartyEdge.LabelName} label");
            await databaseService.ExecuteCustomQuery($"g.E().hasLabel('{TseCandidateBelongsToPartyEdge.LabelName}').drop()");
        }

        private async Task InsertTseBelongsToPartyEdges(List<TseCandidateFileModel> list)
        {

            logService.Log($"Inserting {TseCandidateBelongsToPartyEdge.LabelName} label");
            List<TseCandidateBelongsToPartyEdge> edgeProperties = new List<TseCandidateBelongsToPartyEdge>();
            List<VerticeFilter> fromFilters = new List<VerticeFilter>();
            List<VerticeFilter> toFilters = new List<VerticeFilter>();

            foreach (var item in list) {

                edgeProperties.Add(new TseCandidateBelongsToPartyEdge());
                fromFilters.Add(new VerticeFilter("Cpf", item.Cpf));
                toFilters.Add(new VerticeFilter("PolitcPartyAbbreviation", item.PolitcPartyAbbreviation));
            }

            await databaseService.AddEdges(edgeProperties, TseCandidateBelongsToPartyEdge.LabelName, fromFilters, toFilters);
        }

    }
}
