using PoliWebSearch.Parser.Domain.Edges.Base;

namespace PoliWebSearch.Parser.Domain.Vertices.PoliticalParty
{
    /// <summary>
    /// Political party data representation
    /// </summary>
    public class PoliticalPartyVertice : TseEdge
    {
        /// <summary>
        /// Name of the political party
        /// </summary>
        public string PoliticalPartyName { get; set; }
        /// <summary>
        /// Abrevivation of the political party name
        /// </summary>
        public string PoliticalPartyAbbreviation { get; set; }

        public static string PoliticalPartyAbbreviationPropertyName => nameof(PoliticalPartyAbbreviation);
    }
}
