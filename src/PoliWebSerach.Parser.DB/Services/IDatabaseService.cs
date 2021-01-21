using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services
{
    public interface IDatabaseService
    {
        void Initialize();
        Task AddListOfObjects<T>(List<T> list);
    }
}
