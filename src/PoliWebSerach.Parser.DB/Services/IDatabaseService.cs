using PoliWebSerach.Parser.DB.Operator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services
{
    public interface IDatabaseService
    {
        void Initialize();
        Task AddVertices(List<dynamic> list, string label, string partitionKey);
        Task AddEdges(List<dynamic> list, string label, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter);
    }
}
