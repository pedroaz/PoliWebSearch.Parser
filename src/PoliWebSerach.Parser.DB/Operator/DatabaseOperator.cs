using Dasync.Collections;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliWebSearch.Parser.Infra.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    /// <summary>
    /// Implementation of IDatabaseOperator
    /// </summary>
    public class DatabaseOperator : IDatabaseOperator
    {
        // Services
        private readonly ILogService logService;
        private GremlinServer server;

        /// <summary>
        /// Query list where queries can be stores and later executed 
        /// </summary>
        private List<string> queries = new List<string>();

        /// <summary>
        /// Internal list of failed queries which will be executed multiple times 
        /// </summary>
        private List<string> failedQueries = new List<string>();
        /// <summary>
        /// Internal amount of times the operator already tired to execute the failed queries
        /// </summary>
        private int failureAtempt = 0;

        /// <summary>
        /// Times the operator should try to execute the queries
        /// </summary>
        private const int maxAmountOfFailedAttempts = 6;

        /// <summary>
        /// Internal lock used to add failed queries
        /// </summary>
        private object failureLock = new object();

        /// <summary>
        /// Internal log used to log to console
        /// </summary>
        private float internalLogCounter = 0;

        public DatabaseOperator(ILogService logService)
        {
            this.logService = logService;
        }

        // <inheritdoc/>
        public IDatabaseOperator Initialize(GremlinServer server)
        {
            this.server = server;
            return this;
        }

        // <inheritdoc/>
        public void AddVerticeQuery(dynamic verticeObj, string partitionKey, string label)
        {
            StringBuilder stringBuilder = new StringBuilder($@"g.addV('{label}').property('pk', '{partitionKey}')");
            AddPropertiesToQuery(verticeObj, stringBuilder);
            queries.Add(stringBuilder.ToString());
        }

        // <inheritdoc/>
        public void AddUpsertVerticeQuery(dynamic verticeObj, string partitionKey, string label, VerticeFilter filter)
        {
            // This will only insert the vertice if it wasn't found (using the filder)
            StringBuilder stringBuilder = new StringBuilder($@"g.V().has('{label}', '{filter.PropertyName}', '{filter.PropertyValue}').fold().coalesce(unfold(), addV('{label}').property('pk', '{partitionKey}'))");
            AddPropertiesToQuery(verticeObj, stringBuilder);
            queries.Add(stringBuilder.ToString());
        }

        // <inheritdoc/>
        public void AddEdgeQuery(dynamic edgeObj, string edgeLabel, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter)
        {
            StringBuilder stringBuilder = new StringBuilder($@"g.V().has('{fromVerticeFilter.PropertyName}', '{fromVerticeFilter.PropertyValue}')");
            stringBuilder.Append($@".addE('{edgeLabel}')");
            AddPropertiesToQuery(edgeObj, stringBuilder);
            stringBuilder.Append($@".to(g.V().has('{toVerticeFilter.PropertyName}', '{toVerticeFilter.PropertyValue}'))");
            queries.Add(stringBuilder.ToString());
        }

        // <inheritdoc/>
        public Task<string> GetVertices(VerticeFilter filter)
        {
            throw new NotImplementedException();
        }

        // <inheritdoc/>
        public async Task ExecuteOperations()
        {
            using var client = new GremlinClient(server, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);

            await ExecuteQueryList(client);

            while (failureAtempt < maxAmountOfFailedAttempts && failedQueries.Any()) {
                logService.Log($"Oh no, we are retrying {failedQueries.Count} because they failed. Retry count: {failureAtempt}");
                queries = failedQueries;
                failedQueries = new List<string>();
                await ExecuteQueryList(client);
                failureAtempt++;
            }
        }

        /// <summary>
        /// Execute all queries from the query list
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Log the progress to console
        /// </summary>
        private void InternalLogToConsole()
        {
            internalLogCounter++;
            logService.LogToConsole($"\rPercentage of Execution ({internalLogCounter}/{queries.Count}): {string.Format("{0:P2}.", (internalLogCounter / queries.Count))}");
        }

        /// <summary>
        /// Add the properties to a query using reflection to get public properties from the object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stringBuilder"></param>
        private void AddPropertiesToQuery(dynamic obj, StringBuilder stringBuilder)
        {
            foreach (var property in obj.GetType().GetProperties()) {

                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);

                // As we are setting on the model that this is a list, we should add it to the graph as a list too
                if (property.PropertyType.Name == "List`1") {
                    foreach (var item in propertyValue) {
                        stringBuilder.Append($@".property(list, '{propertyName}', '{item}')");
                    }
                }
                else {
                    // Do not insert empty properties
                    if (propertyValue == string.Empty) continue;
                    stringBuilder.Append($@".property('{propertyName}', '{propertyValue}')");
                }
            }
        }

        // <inheritdoc/>
        public void AddCustomQuery(string customQuery)
        {
            queries.Add(customQuery);
        }

        // <inheritdoc/>
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
