using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Resolver;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSerach.Parser.DB.Operator;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services
{
    public class DatabaseService : IDatabaseService
    {
        private GremlinServer server;
        private readonly IServiceResolver serviceResolver;
        private readonly IConfiguratorService configuratorService;
        private readonly IClockService clockService;

        public DatabaseService(IServiceResolver serviceResolver, IConfiguratorService configuratorService, IClockService clockService)
        {
            this.serviceResolver = serviceResolver;
            this.configuratorService = configuratorService;
            this.clockService = clockService;
        }

        public void Initialize()
        {
            var userName = $"/dbs/{configuratorService.AppConfig.DatabaseName}/colls/{configuratorService.AppConfig.GraphName}";
            server = new GremlinServer(configuratorService.AppConfig.HostName, 443, true, userName, configuratorService.AppConfig.MasterKey);
        }

        public async Task AddVertices(List<dynamic> list, string label, string partitionKey)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            foreach (var item in list) {
                databaseOperator.AddVerticeQuery(item, partitionKey, label);
            }
            await clockService.ExecuteWithStopWatchAsync("Executing AddVertices batch operations on database", async () => {
                await databaseOperator.ExecuteOperations();
            });
        }

        public async Task AddEdges(List<dynamic> list, string label, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            foreach (var item in list) {
                databaseOperator.AddEdgeQuery(item, label, fromVerticeFilter, toVerticeFilter);
            }
            await clockService.ExecuteWithStopWatchAsync("Executing AddEdges batch operations on database", async () => {
                await databaseOperator.ExecuteOperations();
            });
        }
    }
}
