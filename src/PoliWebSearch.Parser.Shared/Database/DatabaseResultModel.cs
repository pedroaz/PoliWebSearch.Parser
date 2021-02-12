using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Domain.Database
{
    public class DatabaseResultModel
    {
        public string id { get; set; }
        public Dictionary<string, List<PropertyPair>> properties { get; set; }
    }
}
