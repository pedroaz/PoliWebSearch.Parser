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

        public IDatabaseOperator AddVertice<T>(T verticeObj, string partitionKey, string label)
        {
            var properties = verticeObj.GetType().GetProperties();
            StringBuilder stringBuilder = new StringBuilder($@"g.addV('{label}').property('pk', '{partitionKey}')");

            foreach (var property in properties) {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(verticeObj);
                stringBuilder.Append($@".property('{propertyName}', '{propertyValue}')");
            }

            queries.Add(stringBuilder.ToString());
            return this;
        }

        public Task GetVertices()
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
    }
}
