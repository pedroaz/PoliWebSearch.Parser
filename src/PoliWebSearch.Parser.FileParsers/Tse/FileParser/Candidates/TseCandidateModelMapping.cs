using CsvHelper.Configuration;
using PoliWebSearch.Parser.Shared.Models.Tse;

namespace PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates
{
    class TseCandidateModelMapping : ClassMap<TseCandidateFileModel>
    {
        public TseCandidateModelMapping()
        {
            Map(m => m.CandidateName).Name("NM_CANDIDATO");
            Map(m => m.Cpf).Name("NR_CPF_CANDIDATO");
            Map(m => m.PolitcPartyAbbreviation).Name("SG_PARTIDO");
            Map(m => m.PolitcPartyName).Name("NM_PARTIDO");
            Map(m => m.SocialCandidateName).Name("NM_SOCIAL_CANDIDATO");
            Map(m => m.CandidateNameOnCedule).Name("NM_URNA_CANDIDATO");
        }
    }
}
