using PoliWebSerach.Parser.DB.Operator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Database
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Executes a custom query into the database
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task ExecuteCustomQuery(string query);

        /// <summary>
        /// Executes a custom query and return the result of it
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<string> ExecuteCustomQueryWithReturnValue(string query);

        /// <summary>
        /// Initialzes the database service
        /// </summary>
        void Initialize();

        /// <summary>
        /// Add a list as vertices into the datbase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="label"></param>
        /// <param name="partitionKey"></param>
        /// <param name="filerName"></param>
        /// <returns></returns>
        Task AddVertices<T>(List<T> list, string label, string partitionKey, string filerName);

        /// <summary>
        /// Add a list as Edges into the database
        /// The from filters and to filters should be ordered in the same order as the edges
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="label"></param>
        /// <param name="fromFilters"></param>
        /// <param name="toFilters"></param>
        /// <returns></returns>
        Task AddEdges<T>(List<T> list, string label, List<VerticeFilter> fromFilters, List<VerticeFilter> toFilters);
    }
}
