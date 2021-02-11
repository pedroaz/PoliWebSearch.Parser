﻿using Gremlin.Net.Driver.Exceptions;
using Newtonsoft.Json;
using PoliWebSearch.Parser.Infra.Services.Log;
using PoliWebSerach.Parser.DB.Services.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Admin
{
    /// <summary>
    /// Type of admin operations
    /// </summary>
    public enum AdminOperations
    {
        count,
        drop
    }

    /// <summary>
    /// Implementation of IAdminService
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly IDatabaseService databaseService;
        private readonly ILogService logService;

        public AdminService(IDatabaseService databaseService, ILogService logService)
        {
            this.databaseService = databaseService;
            this.logService = logService;
        }

        // <inheritdoc/>
        public async Task<long> CountDatabase()
        {
            var result = JsonConvert.DeserializeObject<List<dynamic>>(await databaseService.ExecuteCustomQueryWithReturnValue("g.V().count()"));
            logService.Log($"Current count of the database: {result.First()}", LogType.Admin);
            return result.First();
        }

        // <inheritdoc/>
        public async Task DropDatabase()
        {
            logService.Log("Droping the database. This operation may take a while", LogType.Admin);

            await CountDatabase();
            do {
                logService.Log("Executing the drop query", LogType.Admin);
                try {
                    await databaseService.ExecuteCustomQueryWithReturnValue("g.V().drop()");
                }
                catch (ResponseException) {
                    logService.Log("Server probabbbly timed out. But that's expected, executing the drop query again!", LogType.Admin);
                }
            } while (await CountDatabase() > 0);
        }
    }
}
