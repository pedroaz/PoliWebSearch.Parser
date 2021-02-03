using Gremlin.Net.Driver;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    public interface IDatabaseOperator
    {
        void AddCustomQuery(string customQuery);
        IDatabaseOperator Initialize(GremlinServer server);
        void AddVerticeQuery(dynamic verticeObj, string partitionKey, string label);
        void AddEdgeQuery(dynamic edgeObj, string edgeLabel, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter);
        Task ExecuteOperations();
        Task GetVertices(VerticeFilter filter);
        Task<string> ExecuteCustomQuery(string query);
    }
}
