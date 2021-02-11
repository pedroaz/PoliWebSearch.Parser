using CsvHelper.Configuration;

namespace PoliWebSearch.Parser.Domain.FileParsing.Tse.Candidate
{
    /// <summary>
    /// Mapping between TSE Candidate file with model
    /// </summary>
    public class TseCandidateModelMapping : ClassMap<TseCandidateFileModel>
    {
        public TseCandidateModelMapping()
        {
            // Candidate
            Map(m => m.CandidateName).Name("NM_CANDIDATO");
            Map(m => m.CandidateCpf).Name("NR_CPF_CANDIDATO");
            Map(m => m.CandidateEmail).Name("NM_EMAIL");
            Map(m => m.SocialCandidateName).Name("NM_SOCIAL_CANDIDATO");
            Map(m => m.CandidateNameOnCedule).Name("NM_URNA_CANDIDATO");
            Map(m => m.CandidateNacionality).Name("DS_NACIONALIDADE");
            Map(m => m.CandidateUfOfBirth).Name("SG_UF_NASCIMENTO");
            Map(m => m.CandidateCityOfBirth).Name("NM_UE");
            Map(m => m.CandidateGender).Name("DS_GENERO");
            Map(m => m.CandidateEducationDegree).Name("DS_GRAU_INSTRUCAO");
            Map(m => m.CandidateCivilStatus).Name("DS_ESTADO_CIVIL");
            Map(m => m.CandidateSkinColor).Name("DS_COR_RACA");
            Map(m => m.CandidateOccupation).Name("DS_OCUPACAO");
            Map(m => m.CandidateOccupationCode).Name("CD_OCUPACAO");
            Map(m => m.CandidateMaxSpending).Name("VR_DESPESA_MAX_CAMPANHA");


            // Political Party
            Map(m => m.PoliticalPartyAbbreviation).Name("SG_PARTIDO");
            Map(m => m.PoliticalPartyName).Name("NM_PARTIDO");
            Map(m => m.PoliticalPartyCode).Name("NR_PARTIDO");
            Map(m => m.PoliticalPartyCode).Name("NR_PARTIDO");

            // Election
            Map(m => m.ElectionYear).Name("ANO_ELEICAO");
            Map(m => m.ElectionType).Name("NM_TIPO_ELEICAO");
            Map(m => m.ElectionTypeCode).Name("CD_TIPO_ELEICAO");
            Map(m => m.ElectionDescription).Name("DS_ELEICAO");
            Map(m => m.ElectionDate).Name("DT_ELEICAO");
            Map(m => m.ElectionCode).Name("CD_ELEICAO");


            // UE
            Map(m => m.UeCode).Name("SG_UE");
            Map(m => m.UeName).Name("NM_UE");

            // Political Job
            Map(m => m.PoliticalJobCode).Name("CD_CARGO");
            Map(m => m.PoliticalJobName).Name("DS_CARGO");
        }
    }
}
