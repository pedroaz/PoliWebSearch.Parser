
using PoliWebSearch.Parser.Tse.Service;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.Tse.Service
{
    public interface ITseParserService
    {
        Task<int> ParseFiles(TseDataSourceType dataSource);
    }
}
