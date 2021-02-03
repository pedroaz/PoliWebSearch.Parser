using Gremlin.Net.Driver;
using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Resolver;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSerach.Parser.DB.Operator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Database
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

        public async Task AddVertices<T>(List<T> list, string label, string partitionKey)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            foreach (var item in list) {
                databaseOperator.AddVerticeQuery(item, partitionKey, label);
            }
            await clockService.ExecuteWithStopWatchAsync("Executing AddVertices batch operations on database", async () => {
                await databaseOperator.ExecuteOperations();
            });
        }

        public async Task AddEdges<T>(List<T> list, string label, List<VerticeFilter> fromFilters, List<VerticeFilter> toFilters)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);

            for (int i = 0; i < list.Count; i++) {
                T obj = list[i];
                var fromFilter = fromFilters[i];
                var toFilter = toFilters[i];
                databaseOperator.AddEdgeQuery(obj, label, fromFilter, toFilter);
            }
            await clockService.ExecuteWithStopWatchAsync("Executing AddEdges batch operations on database", async () => {
                await databaseOperator.ExecuteOperations();
            });
        }

        public async Task ExecuteCustomQueries(string query)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            databaseOperator.AddCustomQuery(query);
            await clockService.ExecuteWithStopWatchAsync($"Executing Custom query database: {query}", async () => {
                await databaseOperator.ExecuteOperations();
            });

        }

        public async Task<string> ExecuteCustomQuery(string query)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            return await databaseOperator.ExecuteCustomQuery(query);
        }
    }
}
