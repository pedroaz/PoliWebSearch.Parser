using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Domain.FileParsing.Tse.CandidateAssets
{
    public class TseCandidateAssetsFileModel
    {
        // Election
        public string ElectionYear { get; set; }
        public string ElectionCode { get; set; }
        public string ElectionDescription { get; set; }
        public string ElectionDate { get; set; }
        public string ElectionUF { get; set; }

        // Candidate
        public string InternalSequentialId { get; set; }

        // Asset
        public string AssetTypeCode { get; set; }
        public string AssetTypeDescription { get; set; }
        public string AssetDescription { get; set; }
        public string AssetValue { get; set; }
    }
}
