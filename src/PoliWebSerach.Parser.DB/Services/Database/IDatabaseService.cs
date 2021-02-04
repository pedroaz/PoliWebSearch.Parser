using PoliWebSerach.Parser.DB.Operator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Database
{
    public interface IDatabaseService
    {
        Task ExecuteCustomQueries(string query);
        Task<string> ExecuteCustomQuery(string query);
        void Initialize();
        Task AddVertices<T>(List<T> list, string label, string partitionKey, string filerName);
        Task AddEdges<T>(List<T> list, string label, List<VerticeFilter> fromFilters, List<VerticeFilter> toFilters);
    }
}
