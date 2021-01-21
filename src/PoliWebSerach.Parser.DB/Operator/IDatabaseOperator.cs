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
        void AddVerticeQuery(dynamic verticeObj, string partitionKey, string label);
        void AddEdgeQuery(dynamic edgeObj, string edgeLabel, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter);
        Task ExecuteOperations();
        Task GetVertices(VerticeFilter filter);
    }
}
