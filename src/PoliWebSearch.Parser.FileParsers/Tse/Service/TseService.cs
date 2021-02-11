using PoliWebSearch.Parser.FileParsers.Tse.Candidate;
using PoliWebSearch.Parser.Infra.Services.Log;
using PoliWebSerach.Parser.DB.Services.Admin;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.Service
{
    public enum TseDataSourceType
    {
        candidatos,
        resultados
    }

    public class TseService : ITseService
    {
        private readonly ILogService logService;
        private readonly IAdminService adminService;
        private readonly ITseCandidateParser candidateParser;

        public TseService(ILogService logService, IAdminService adminService, ITseCandidateParser candidateParser)
        {
            this.logService = logService;
            this.adminService = adminService;
            this.candidateParser = candidateParser;
        }

        // <inheritdoc/>
        public async Task<int> ParseFiles(TseDataSourceType dataSource, int rowLimit, bool dropFirst)
        {
            logService.Log("Starting to parse tse files");

            if (dropFirst) await DropDatabase();

            switch (dataSource) {
                case TseDataSourceType.candidatos:
                    await candidateParser.ParseCandidates(rowLimit);
                    break;
                case TseDataSourceType.resultados:
                    break;
            }

            return 0;
        }

        /// <summary>
        /// Drop the entire database
        /// </summary>
        /// <returns></returns>
        private async Task DropDatabase()
        {
            logService.Log("Droping the database before inserting");
            await adminService.DropDatabase();
        }
    }
}
