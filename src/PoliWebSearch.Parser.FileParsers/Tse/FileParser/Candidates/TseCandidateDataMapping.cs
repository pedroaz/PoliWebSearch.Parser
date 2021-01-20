using PoliWebSearch.Parser.Shared.Models;
using TinyCsvParser.Mapping;

namespace PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates
{
    class TseCandidateDataMapping : CsvMapping<TseCandidateModel>
    {
        public TseCandidateDataMapping() : base()
        {
            MapProperty(17, x => x.CandidateName);
            MapProperty(20, x => x.Cpf);
        }
    }
}
