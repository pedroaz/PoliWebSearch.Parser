using PoliWebSearch.Parser.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Infra.Services.Result
{
    public interface IResultService
    {
        /// <summary>
        /// Add that the database was counted
        /// </summary>
        /// <param name="value"></param>
        void AddDatabaseCount(long value);

        /// <summary>
        /// Add the result of a custom query
        /// </summary>
        /// <param name="result"></param>
        void AddCustomQueryResult(string query, List<DatabaseResultModel> result);
    }
}
