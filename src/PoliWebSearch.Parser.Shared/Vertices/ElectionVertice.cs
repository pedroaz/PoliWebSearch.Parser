using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Domain.Vertices
{
    /// <summary>
    /// Vertice which represents an election
    /// </summary>
    public class ElectionVertice
    {
        public string Code { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string DateOfElection { get; set; }
    }
}
