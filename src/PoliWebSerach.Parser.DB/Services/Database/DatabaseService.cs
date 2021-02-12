using Gremlin.Net.Driver;
using Newtonsoft.Json;
using PoliWebSearch.Parser.Domain.Database;
using PoliWebSearch.Parser.Infra.Configurator;
using PoliWebSearch.Parser.Infra.Resolver;
using PoliWebSearch.Parser.Infra.Services.Clock;
using PoliWebSerach.Parser.DB.Operator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Database
{
    /// <summary>
    /// Implementation of IDatabaseService
    /// </summary>
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

        // <inheritdoc/>
        public void Initialize()
        {
            var userName = $"/dbs/{configuratorService.AppConfig.DatabaseName}/colls/{configuratorService.AppConfig.GraphName}";
            server = new GremlinServer(configuratorService.AppConfig.HostName, 443, true, userName, configuratorService.AppConfig.MasterKey);
        }

        // <inheritdoc/>
        public async Task AddVertices<T>(List<T> listOfObjects, string label, string partitionKey, string filterName)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);

            foreach (var obj in listOfObjects) {
                databaseOperator.AddUpsertVerticeQuery(obj, partitionKey, label, GetVerticeFilter(obj, filterName));
            }

            await clockService.ExecuteWithStopWatchAsync("Executing AddVertices batch operations on database", async () => {
                await databaseOperator.ExecuteOperations();
            });
        }

        /// <summary>
        /// Create the vertice filter by using the filter name and reading the value using reflection
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filterName"></param>
        /// <returns></returns>
        private VerticeFilter GetVerticeFilter(object obj, string filterName)
        {
            foreach (var property in obj.GetType().GetProperties()) {
                if (property.Name == filterName) {
                    return new VerticeFilter(filterName, property.GetValue(obj).ToString());
                }
            }
            // This should never happen - You should not pass a filter name which the object does not contain
            throw new Exception("Wrong filter name given on a insert");
        }

        // <inheritdoc/>
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

        // <inheritdoc/>
        public async Task ExecuteCustomQuery(string query)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            databaseOperator.AddCustomQuery(query);
            await clockService.ExecuteWithStopWatchAsync($"Executing Custom query database: {query}", async () => {
                await databaseOperator.ExecuteOperations();
            });

        }

        // <inheritdoc/>
        public async Task<string> ExecuteCustomQueryWithReturnValue(string query)
        {
            var databaseOperator = serviceResolver.ResolveService<IDatabaseOperator>().Initialize(server);
            return await databaseOperator.ExecuteCustomQuery(query);
        }

        public List<DatabaseResultModel> ConvertResultToModel(string jsonResult)
        {
            return JsonConvert.DeserializeObject<List<DatabaseResultModel>>(jsonResult);
        }
    }
}
