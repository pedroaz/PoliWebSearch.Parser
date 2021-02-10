﻿using PoliWebSearch.Parser.Shared.Models.Tse;

namespace PoliWebSearch.Parser.Shared.Models.Edges
{
    /// <summary>
    /// Relationship between candidate and party
    /// </summary>
    public class TseCandidateBelongsToPartyEdge : TseEdge
    {
        /// <summary>
        /// Name of the label
        /// </summary>
        public static string LabelName = "tseBelongsToParty";

        /// <summary>
        /// Year where the candidate was from the party
        /// </summary>
        public string Year { get; set; }
    }
}
