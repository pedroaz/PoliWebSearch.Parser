using PoliWebSearch.Parser.Shared.Services.Log;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PoliWebSearch.Parser.Shared.Services.File
{
    public class FileService : IFileService
    {
        private readonly ILogService logService;

        public FileService(ILogService logService)
        {
            this.logService = logService;
        }

        public bool DirExists(string dirPath)
        {
            logService.Log($"Looking for folder in {dirPath}");
            if (!Directory.Exists(dirPath)) {
                logService.Log($"Directory does not exists.");
                return false;
            }
            logService.Log($"Found directory");
            return true;
        }

        public List<string> GetFilesFromDir(string dirPath)
        {
            logService.Log($"Searching on {dirPath} for files");
            var files = Directory.GetFiles(dirPath);
            logService.Log($"Found {files.Length} inside this folder");
            return files.ToList();
        }
    }
}
