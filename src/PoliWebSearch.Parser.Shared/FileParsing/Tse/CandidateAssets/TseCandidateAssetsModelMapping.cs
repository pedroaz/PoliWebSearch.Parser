using CsvHelper.Configuration;

namespace PoliWebSearch.Parser.Domain.FileParsing.Tse.CandidateAssets
{
    public class TseCandidateAssetsModelMapping : ClassMap<TseCandidateAssetsFileModel>
    {
        public TseCandidateAssetsModelMapping()
        {
            // Candidate
            Map(m => m.InternalSequentialId).Name("SQ_CANDIDATO");
        }
    }
}
