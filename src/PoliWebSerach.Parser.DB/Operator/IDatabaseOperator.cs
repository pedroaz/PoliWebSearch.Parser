using Gremlin.Net.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    public interface IDatabaseOperator
    {
        void Initialize(GremlinServer server);
        Task AddVertice<T>(T verticeObj, string partitionKey);
        Task GetVertices();
    }
}
