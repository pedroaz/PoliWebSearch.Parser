using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreProperty : Attribute
    {
        public IgnoreProperty()
        {
        }
    }
}
