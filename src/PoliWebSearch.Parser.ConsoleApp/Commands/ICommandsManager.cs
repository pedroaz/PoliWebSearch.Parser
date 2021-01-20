using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp.Commands
{
    public interface ICommandsManager
    {
        Task Loop();
    }
}
