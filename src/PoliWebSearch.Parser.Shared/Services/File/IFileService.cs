using System.Collections.Generic;

namespace PoliWebSearch.Parser.Shared.Services.File
{
    public interface IFileService
    {
        List<string> GetFilesFromDir(string dirPath);
        bool DirExists(string dirPath);
    }
}
