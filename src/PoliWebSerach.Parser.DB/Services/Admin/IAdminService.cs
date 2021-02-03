using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services.Admin
{
    public interface IAdminService
    {
        Task<long> CountDatabase();
        Task DropDatabase();
    }
}
