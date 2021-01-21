using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    public class DatabaseOperator : IDatabaseOperator
    {
        public string operationGUID = Guid.NewGuid().ToString();
        private GremlinServer server;
        private List<string> queries = new List<string>();

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

            foreach (var query in queries) {
                await client.SubmitAsync(query);
            }
        }


        private void AddPropertiesToQuery(dynamic obj, StringBuilder stringBuilder)
        {
            foreach (var property in obj.GetType().GetProperties()) {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);
                stringBuilder.Append($@".property('{propertyName}', '{propertyValue}')");
            }
        }
    }
}
