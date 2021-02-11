namespace PoliWebSearch.Parser.FileParsers.Tse.Candidate
{
    /// <summary>
    /// Model for the Tse Cadidate File
    /// </summary>
    public class TseCandidateFileModel
    {
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
        public string PolitcPartyName { get; set; }
        public string PolitcPartyAbbreviation { get; set; }
    }
}
