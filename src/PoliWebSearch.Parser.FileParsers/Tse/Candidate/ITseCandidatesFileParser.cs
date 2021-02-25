using PoliWebSearch.Parser.Domain.FileParsing.Tse.Candidate;
using System.Collections.Generic;

namespace PoliWebSearch.Parser.FileParsers.Tse.Candidate
{
    /// <summary>
    /// Parser service for TSE candidate files
    /// </summary>
    public interface ITseCandidatesFileParser
    {
        /// <summary>
        /// Parse the TSE candidate file and return the model
        /// </summary>
        /// <param name="filePath">File which will be parsed</param>
        /// <returns>Parsed model</returns>
        List<TseCandidateFileModel> ParseCandidateFile(string filePath);
    }
}
