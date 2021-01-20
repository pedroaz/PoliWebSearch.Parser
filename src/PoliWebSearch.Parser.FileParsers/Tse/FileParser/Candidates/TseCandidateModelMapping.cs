using CsvHelper.Configuration;
using PoliWebSearch.Parser.Shared.Models;

namespace PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates
{
    class TseCandidateModelMapping : ClassMap<TseCandidateModel>
    {
        public TseCandidateModelMapping()
        {
            Map(m => m.CandidateName).Name("NM_CANDIDATO");
            Map(m => m.Cpf).Name("NR_CPF_CANDIDATO");
        }
    }
}
