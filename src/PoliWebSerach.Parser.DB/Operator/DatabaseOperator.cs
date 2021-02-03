using Dasync.Collections;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliWebSearch.Parser.Shared.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    public class DatabaseOperator : IDatabaseOperator
    {

        private readonly ILogService logService;

        public string operationGUID = Guid.NewGuid().ToString();
        private GremlinServer server;
        private List<string> queries = new List<string>();

        private List<string> failedQueries = new List<string>();
        private int failureAtempt = 0;
        private object failureLock = new object();
        private int internalLogCounter = 0;

        public DatabaseOperator(ILogService logService)
        {
            this.logService = logService;
        }

        public IDatabaseOperator Initialize(GremlinServer server)
        {
            this.server = server;
            return this;
        }

        public void AddVerticeQuery(dynamic verticeObj, string partitionKey, string label)
        {
            StringBuilder stringBuilder = new StringBuilder($@"g.addV('{label}').property('pk', '{partitionKey}')");
            AddPropertiesToQuery(verticeObj, stringBuilder);
            queries.Add(stringBuilder.ToString());
        }

        public void AddEdgeQuery(dynamic edgeObj, string edgeLabel, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter)
        {
            StringBuilder stringBuilder = new StringBuilder($@"g.V().has('{fromVerticeFilter.PropertyName}', '{fromVerticeFilter.PropertyValue}')");
            stringBuilder.Append($@".addE('{edgeLabel}')");
            AddPropertiesToQuery(edgeObj, stringBuilder);
            stringBuilder.Append($@".to(g.V().has('{toVerticeFilter.PropertyName}', '{toVerticeFilter.PropertyValue}'))");
            queries.Add(stringBuilder.ToString());
        }

        public Task GetVertices(VerticeFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteOperations()
        {
            using var client = new GremlinClient(server, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);

            await ExecuteQueryList(client);

            while (failureAtempt < 6 && failedQueries.Any()) {
                logService.Log($"Oh no, we are retrying {failedQueries.Count} because they failed. Retry count: {failureAtempt}");
                queries = failedQueries;
                failedQueries = new List<string>();
                await ExecuteQueryList(client);
                failureAtempt++;
            }
        }

        private async Task ExecuteQueryList(GremlinClient client)
        {
            logService.Log($"Executing {queries.Count} queries");

            await queries.ParallelForEachAsync(async (query) => {
                try {
                    await client.SubmitAsync(query);
                    InternalLogToConsole();
                }
                catch (Exception e) {

                    logService.Log($"Failed to execute query: {query}");
                    logService.Log(e.Message);
                    lock (failureLock) {
                        failedQueries.Add(query);
                    }
                }
            }, maxDegreeOfParallelism: 10);
        }

        private void InternalLogToConsole()
        {
            internalLogCounter++;
            var number = (internalLogCounter / (float)queries.Count);
            var sformatted = string.Format("{0:0.##\\%}", number);
            logService.LogToConsole($"\rPercentage of Execution ({internalLogCounter}/{queries.Count}): {sformatted}");
        }

        private void AddPropertiesToQuery(dynamic obj, StringBuilder stringBuilder)
        {
            foreach (var property in obj.GetType().GetProperties()) {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);
                stringBuilder.Append($@".property('{propertyName}', '{propertyValue}')");
            }
        }

        public void AddCustomQuery(string customQuery)
        {
            queries.Add(customQuery);
        }

        public async Task<string> ExecuteCustomQuery(string query)
        {
            using var client = new GremlinClient(server, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
            var resultSet = await client.SubmitAsync<dynamic>(query);
            JArray array = new JArray();
            foreach (var result in resultSet) {

                string jsonObject = JsonConvert.SerializeObject(result, new JsonSerializerSettings() {
                    Formatting = Formatting.Indented
                });
                array.Add(JsonConvert.DeserializeObject(jsonObject));
            }
            return array.ToString();
        }
    }
}
