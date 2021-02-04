using PoliWebSearch.Parser.Shared.Models.Tse;

namespace PoliWebSearch.Parser.Shared.Models.Edges
{
    public class TseCandidateBelongsToPartyEdge : TseDataModel
    {
        public static string LabelName = "tseBelongsToParty";

        public string Year { get; set; }
    }
}
