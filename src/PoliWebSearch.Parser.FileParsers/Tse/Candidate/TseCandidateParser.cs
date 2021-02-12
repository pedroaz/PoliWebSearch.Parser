using MoreLinq;
using PoliWebSearch.Parser.Domain.Edges.Tse;
using PoliWebSearch.Parser.Domain.FileParsing.Tse.Candidate;
using PoliWebSearch.Parser.Domain.Vertices;
using PoliWebSearch.Parser.Infra.Configurator;
using PoliWebSearch.Parser.Infra.Services.Clock;
using PoliWebSearch.Parser.Infra.Services.IO;
using PoliWebSearch.Parser.Infra.Services.Log;
using PoliWebSerach.Parser.DB.Operator;
using PoliWebSerach.Parser.DB.Services.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.Candidate
{
    /// <summary>
    /// Implementation of ITseCandidateParser
    /// </summary>
    public class TseCandidateParser : ITseCandidateParser
    {
        private readonly ILogService logService;
        private readonly IConfiguratorService configurator;
        private readonly ITseCandidatesFileParser candidatesFileParser;
        private readonly IFileService fileService;
        private readonly IClockService clockService;
        private readonly IDatabaseService databaseService;

        public TseCandidateParser(ILogService logService, IConfiguratorService configurator, ITseCandidatesFileParser candidatesFileParser,
            IFileService fileService, IClockService clockService, IDatabaseService databaseService)
        {
            this.logService = logService;
            this.configurator = configurator;
            this.candidatesFileParser = candidatesFileParser;
            this.fileService = fileService;
            this.clockService = clockService;
            this.databaseService = databaseService;
        }

        /// <summary>
        /// Parse all candidate files and insert into the database
        /// </summary>
        /// <param name="rowLimit"></param>
        /// <returns></returns>
        public async Task ParseCandidates(int rowLimit)
        {
            // Parse files
            string dirPath = Path.Join(configurator.AppConfig.StorageDirectory, "Tse", "Candidatos");
            if (!fileService.DirExists(dirPath)) return;
            var files = fileService.GetFilesFromDir(dirPath);
            List<TseCandidateFileModel> list = new List<TseCandidateFileModel>();
            clockService.ExecuteWithStopWatch("Parsing TSE Candidates", () => {
                foreach (var file in files) {
                    logService.Log($"Parsing File: {file}");
                    list.AddRange(candidatesFileParser.ParseCandidateFile(file));
                }
            });

            // Remove rows if needed
            if (rowLimit > 0) {
                list = list.Take(rowLimit).ToList();
            }

            await InsertDataIntoDatabase(list);
        }

        /// <summary>
        /// Insert all candidates data into the database
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private async Task InsertDataIntoDatabase(List<TseCandidateFileModel> list)
        {
            logService.Log($"Amount of records {list.Count}");
            await InserPeopleVertices(list);
            await InserPoliticalPartyVertices(list);
            await RemoveTseLabels();
            await InsertTseBelongsToPartyEdges(list);
        }

        /// <summary>
        /// Insert all people vertices into the database
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private async Task InserPeopleVertices(List<TseCandidateFileModel> list)
        {
            logService.Log("Inserting People Vertices");
            var peopleList = list.Select(x =>
                            new PersonVertice() {
                                Names = new List<string>() { x.CandidateName, x.SocialCandidateName, x.CandidateNameOnCedule },
                                Cpf = x.CandidateCpf,
                                BirthDate = x.CandidateBirthDate,
                                Emails = new List<string>() { x.CandidateEmail },
                                Genders = new List<string>() { x.CandidateGender }
                            }
                        ).ToList();

            await databaseService.AddVertices(peopleList, "person", "1", PersonVertice.CpfPropertyName);
        }

        /// <summary>
        /// Insert all polical party into the database
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private async Task InserPoliticalPartyVertices(List<TseCandidateFileModel> list)
        {
            logService.Log("Inserting Political Party Vertices");
            var partyList = list.Select(x =>
                            new PoliticalPartyVertice() {
                                PoliticalPartyAbbreviation = x.PoliticalPartyAbbreviation,
                                PoliticalPartyName = x.PoliticalPartyName
                            }
                        ).ToList();

            partyList = partyList.DistinctBy(x => x.PoliticalPartyAbbreviation).ToList();

            await databaseService.AddVertices(partyList, "political_party", "1", PoliticalPartyVertice.PoliticalPartyAbbreviationPropertyName);
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
                fromFilters.Add(new VerticeFilter(PersonVertice.CpfPropertyName, item.CandidateCpf));
                toFilters.Add(new VerticeFilter(PoliticalPartyVertice.PoliticalPartyAbbreviationPropertyName, item.PoliticalPartyAbbreviation));
            }

            await databaseService.AddEdges(edgeProperties, TseCandidateBelongsToPartyEdge.LabelName, fromFilters, toFilters);
        }
    }
}
