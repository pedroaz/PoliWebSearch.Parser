namespace PoliWebSearch.Parser.Domain.FileParsing.Tse.Candidate
{
    /// <summary>
    /// Model for the Tse Cadidate File
    /// </summary>
    public class TseCandidateFileModel
    {
        // Candidate
        public string CandidateName { get; set; }
        public string CandidateNameOnCedule { get; set; }
        private string socialName = "";
        public string SocialCandidateName
        {
            get
            {
                return socialName;
            }
            set
            {
                socialName = value == "#NULO#" ? "" : value;
            }
        }
        public string Cpf { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidateNacionality { get; set; }

        // Political Party
        public string PoliticalPartyName { get; set; }
        public string PoliticalPartyAbbreviation { get; set; }
        public string PoliticalPartyCode { get; set; }

        // Election
        public string ElectionYear { get; set; }
        public string ElectionCode { get; set; }
        public string ElectionType { get; set; }
        public string ElectionTypeCode { get; set; }
        public string ElectionDescription { get; set; }
        public string ElectionDate { get; set; }

        // UE
        public string UeCode { get; set; }
        public string UeName { get; set; }

        // Political Job
        public string PoliticalJobCode { get; set; }
        public string PoliticalJobName { get; set; }
        
    }
}
