using PoliWebSearch.Parser.Shared.Models.Tse;
using System.Collections.Generic;

namespace PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates
{
    public interface ITseCandidatesFileParser
    {
        List<TseCandidateFileModel> ParseFile(string filePath);
    }
}
