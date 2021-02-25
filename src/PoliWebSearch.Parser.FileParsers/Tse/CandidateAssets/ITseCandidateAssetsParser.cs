using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.CandidateAssets
{
    public interface ITseCandidateAssetsParser
    {
        Task ParseCandidateAssets(int rowLimit);
    }
}
