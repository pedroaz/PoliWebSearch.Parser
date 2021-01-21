using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using System;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    public class DatabaseOperator : IDatabaseOperator
    {
        public string operationGUID = Guid.NewGuid().ToString();
        private GremlinServer server;

        public void Initialize(GremlinServer server)
        {
            this.server = server;
        }

        public async Task AddVertice<T>(T verticeObj, string partitionKey)
        {
            using var client = new GremlinClient(server, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);

            var query = $@"
                    g.V()
                ";

            var results = await client.SubmitAsync<dynamic>(query);
        }

        public Task GetVertices()
        {
            throw new NotImplementedException();
        }
    }
}
