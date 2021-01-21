using Gremlin.Net.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    public interface IDatabaseOperator
    {
        IDatabaseOperator Initialize(GremlinServer server);
        IDatabaseOperator AddVertice<T>(T verticeObj, string partitionKey, string label);
        Task ExecuteOperations();
        Task GetVertices();
    }
}
