using PoliWebSearch.Parser.Domain.Tse;

namespace PoliWebSearch.Parser.Domain.PoliticalParty
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
