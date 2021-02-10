using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.Candidate
{
    /// <summary>
    /// Handles the parsing of the candidates data from TSE
    /// </summary>
    public interface ITseCandidateParser
    {
        Task ParseCandidates(int rowLimit);
    }
}
