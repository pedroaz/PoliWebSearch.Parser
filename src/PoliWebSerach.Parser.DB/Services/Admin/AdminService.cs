using Gremlin.Net.Driver.Exceptions;
using Newtonsoft.Json;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSerach.Parser.DB.Services.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Admin
{
    public enum AdminOperations
    {
        count,
        drop
    }

    public class AdminService : IAdminService
    {
        private readonly IDatabaseService databaseService;
        private readonly ILogService logService;

        public AdminService(IDatabaseService databaseService, ILogService logService)
        {
            this.databaseService = databaseService;
            this.logService = logService;
        }

        public async Task<long> CountDatabase()
        {
            var result = JsonConvert.DeserializeObject<List<dynamic>>(await databaseService.ExecuteCustomQueryWithReturnValue("g.V().count()"));
            logService.Log($"Current count of the database: {result.First()}");
            return result.First();
        }

        public async Task DropDatabase()
        {

            logService.Log("Droping the database. This operation may take a while");

            await CountDatabase();
            do {
                logService.Log("Executing the drop query");
                try {
                    await databaseService.ExecuteCustomQueryWithReturnValue("g.V().drop()");
                }
                catch (ResponseException) {
                    logService.Log("Server probabbbly timed out. But that's expected, executing the drop query again!");
                }
            } while (await CountDatabase() > 0);
        }
    }
}
