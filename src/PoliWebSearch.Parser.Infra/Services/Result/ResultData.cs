using System.Collections.Generic;

namespace PoliWebSearch.Parser.Infra.Services.Result
{
    public class ResultData
    {
        public List<long> GraphCounts { get; set; }

        public ResultData()
        {
            GraphCounts = new List<long>();
        }
    }
}
