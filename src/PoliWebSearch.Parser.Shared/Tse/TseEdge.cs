using PoliWebSearch.Parser.Domain.Edges;

namespace PoliWebSearch.Parser.Domain.Tse
{
    /// <summary>
    /// Represents and edge whcih came from the TSE data
    /// </summary>
    public class TseEdge : EdgeDataModel
    {
        public override string DataSource => "TSE";
    }
}
