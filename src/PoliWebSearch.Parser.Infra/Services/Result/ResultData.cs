using PoliWebSearch.Parser.Domain.Database;
using System.Collections.Generic;

namespace PoliWebSearch.Parser.Infra.Services.Result
{
    public class ResultData
    {
        public List<long> GraphCounts { get; set; }
        public Dictionary<string, List<DatabaseResultModel>> QueryResults { get; set; }

        public ResultData()
        {
            GraphCounts = new List<long>();
            QueryResults = new Dictionary<string, List<DatabaseResultModel>>();
        }
    }
}
