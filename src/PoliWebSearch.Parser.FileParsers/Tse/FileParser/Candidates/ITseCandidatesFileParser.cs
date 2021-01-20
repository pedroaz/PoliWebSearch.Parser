using PoliWebSearch.Parser.Shared.Models;
using System.Collections.Generic;

namespace PoliWebSearch.Parser.Tse.FileParsers.Candidates
{
    public interface ITseCandidatesFileParser
    {
        List<TseCandidateModel> ParseFile(string filePath);
    }
}
