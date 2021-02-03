using Newtonsoft.Json;
using PoliWebSearch.Parser.Shared.Services.Log;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace PoliWebSerach.Parser.DB.Services
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
            var result = JsonConvert.DeserializeObject<List<dynamic>>(await databaseService.ExecuteCustomQuery("g.V().count()"));
            logService.Log($"Current count of the database: {result.First()}");
            return result.First();
        }

        public async Task DropDatabase()
        {
            await CountDatabase();
            do {
                await databaseService.ExecuteCustomQuery("g.V().drop()");
            } while (await CountDatabase() > 0);
        }
    }
}
