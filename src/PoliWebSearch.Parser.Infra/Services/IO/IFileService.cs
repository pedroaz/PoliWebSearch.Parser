using System.Collections.Generic;

namespace PoliWebSearch.Parser.Infra.Services.IO
{
    /// <summary>
    /// Useful File operations (System.IO)
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Returns a list of string which are all files from certain dir
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        List<string> GetFilesFromDir(string dirPath);

        /// <summary>
        /// Checks if certain directory exists
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        bool DirExists(string dirPath);


        /// <summary>
        /// Get amount of file of a directory
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        int GetAmountOfFilesInDir(string dirPath);


        /// <summary>
        /// Write string to a file
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        void WriteAllText(string content, string path);

    }
}
