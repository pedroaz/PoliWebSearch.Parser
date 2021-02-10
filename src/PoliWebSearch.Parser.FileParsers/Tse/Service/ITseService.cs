using System.Threading.Tasks;

namespace PoliWebSearch.Parser.FileParsers.Tse.Service
{
    /// <summary>
    /// Parser for TSE files
    /// </summary>
    public interface ITseService
    {
        /// <summary>
        /// Parse TSE files (which will be decided by the data source
        /// </summary>
        /// <param name="dataSource">What kind of file will be parsed</param>
        /// <param name="rowLimit">Limit of file rows</param>
        /// <param name="dropFirst">If the database should be droped before inserting</param>
        /// <returns></returns>
        Task<int> ParseFiles(TseDataSourceType dataSource, int rowLimit, bool dropFirst);
    }
}
