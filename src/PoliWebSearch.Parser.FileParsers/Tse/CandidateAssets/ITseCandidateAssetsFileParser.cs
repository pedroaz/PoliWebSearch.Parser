using PoliWebSearch.Parser.Domain.FileParsing.Tse.CandidateAssets;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.FileParsers.Tse.CandidateAssets
{
    /// <summary>
    /// Parser service for TSE candidate asset file
    /// </summary>
    public interface ITseCandidateAssetsFileParser
    {
        /// <summary>
        /// Parse the TSE candidate assets file and return the model
        /// </summary>
        /// <param name="filePath">File which will be parsed</param>
        /// <returns>Parsed model</returns>
        List<TseCandidateAssetsFileModel> ParseCandidateAssetsFile(string filePath);
    }
}
