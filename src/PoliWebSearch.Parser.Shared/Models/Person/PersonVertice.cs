using System.Collections.Generic;

namespace PoliWebSearch.Parser.Shared.Models.Person
{
    public class PersonVertice : GeneralDataModel
    {
        public List<string> Names { get; set; }
        public string Cpf { get; set; }
    }
}
