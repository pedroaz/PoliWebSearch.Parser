using CsvHelper.Configuration;

namespace PoliWebSearch.Parser.Domain.FileParsing.Tse.CandidateAssets
{
    public class TseCandidateAssetsModelMapping : ClassMap<TseCandidateAssetsFileModel>
    {
        public TseCandidateAssetsModelMapping()
        {
            
            // Election
            Map(m => m.ElectionYear).Name("ANO_ELEICAO");
            Map(m => m.ElectionCode).Name("CD_ELEICAO");
            Map(m => m.ElectionDescription).Name("DS_ELEICAO");
            Map(m => m.ElectionDate).Name("DT_ELEICAO");
            Map(m => m.ElectionUF).Name("SG_UF");


            // Candidate
            Map(m => m.InternalSequentialId).Name("SQ_CANDIDATO");

            // Asset
            Map(m => m.AssetTypeCode).Name("CD_TIPO_BEM_CANDIDATO");
            Map(m => m.AssetTypeDescription).Name("DS_TIPO_BEM_CANDIDATO");
            Map(m => m.AssetDescription).Name("DS_BEM_CANDIDATO");
            Map(m => m.AssetValue).Name("VR_BEM_CANDIDATO");
        }
    }
}
