using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services
{
    public interface IAdminService
    {
        Task<long> CountDatabase();
        Task DropDatabase();
    }
}
