using System;

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
        public string CandidateCpf { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidateNacionality { get; set; }
        public string CandidateUfOfBirth { get; set; }
        public string CandidateCityOfBirth { get; set; }
        public string CandidateGender { get; set; }
        public string CandidateEducationDegree { get; set; }
        public string CandidateCivilStatus { get; set; }
        public string CandidateSkinColor { get; set; }
        public string CandidateOccupation { get; set; }
        public string CandidateOccupationCode { get; set; }
        public string CandidateBirthDate { get; set; }
        public string InternalSequentialId { get; set; }

        public string CandidateMaxSpending { get; set; }

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

        // Gender

    }
}
