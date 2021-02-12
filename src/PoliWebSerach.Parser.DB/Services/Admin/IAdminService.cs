using PoliWebSearch.Parser.Domain.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Admin
{
    /// <summary>
    /// Service used to process admin operations into the database
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Get amount of values of the entire database
        /// </summary>
        /// <returns></returns>
        Task<long> CountDatabase();
        /// <summary>
        /// Drop the entire database
        /// </summary>
        /// <returns></returns>
        Task DropDatabase();
        /// <summary>
        /// Execute a custom operation on the database and returns the result
        /// </summary>
        /// <returns></returns>
        Task<List<DatabaseResultModel>> ExecuteCustomOperation(string operation);
    }
}
