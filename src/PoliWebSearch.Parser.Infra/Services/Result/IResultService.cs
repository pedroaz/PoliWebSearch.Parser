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
    }
}
