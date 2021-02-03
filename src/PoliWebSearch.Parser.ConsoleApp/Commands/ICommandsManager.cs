using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp.Commands
{
    public interface ICommandsManager
    {
        Task Loop();
    }
}
