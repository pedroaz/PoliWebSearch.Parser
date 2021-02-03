using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.Service
{
    public interface ITseParserService
    {
        Task<int> ParseFiles(TseDataSourceType dataSource, int rowLimit, bool dropFirst);
    }
}
