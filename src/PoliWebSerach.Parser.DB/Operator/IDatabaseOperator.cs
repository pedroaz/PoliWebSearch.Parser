using Gremlin.Net.Driver;
using PoliWebSearch.Parser.Domain.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Operator
{
    /// <summary>
    /// Database operator to store queries and execute them.
    /// Will use one client per operator
    /// How to use: 
    /// 1) Initialize the operator with the server
    /// 2) Add queries (all whcih should be executed)
    /// 3) Execute them
    /// OR
    /// Use an Get operation to execute a query right away
    /// </summary>
    public interface IDatabaseOperator
    {
        /// <summary>
        /// Add a custom query to the query list
        /// </summary>
        /// <param name="customQuery"></param>
        void AddCustomQuery(string customQuery);

        /// <summary>
        /// Initializes the operator with the server
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        IDatabaseOperator Initialize(GremlinServer server);

        /// <summary>
        /// Add a custom vertice query
        /// </summary>
        /// <param name="verticeObj"></param>
        /// <param name="partitionKey"></param>
        /// <param name="label"></param>
        void AddVerticeQuery(dynamic verticeObj, string partitionKey, string label);

        /// <summary>
        /// Add a custom edge query
        /// </summary>
        /// <param name="edgeObj"></param>
        /// <param name="edgeLabel"></param>
        /// <param name="fromVerticeFilter"></param>
        /// <param name="toVerticeFilter"></param>
        void AddEdgeQuery(dynamic edgeObj, string edgeLabel, VerticeFilter fromVerticeFilter, VerticeFilter toVerticeFilter);

        /// <summary>
        /// Execut all the operations that are store on the operator
        /// </summary>
        /// <returns></returns>
        Task ExecuteOperations();

        /// <summary>
        /// Get all vertices that match the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<string> GetVertices(VerticeFilter filter);

        /// <summary>
        /// Execute a custom filter an return the result as a string
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<string> ExecuteCustomQuery(string query);

        /// <summary>
        /// Add a upsert vertice query using the filter
        /// </summary>
        /// <param name="verticeObj"></param>
        /// <param name="partitionKey"></param>
        /// <param name="label"></param>
        /// <param name="filter"></param>
        void AddUpsertVerticeQuery(dynamic verticeObj, string partitionKey, string label, VerticeFilter filter);
    }
}
