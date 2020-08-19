using System;
using System.Collections.Generic;
using System.Text;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration
{
    public class EnumerationType
    {
        public string Name { get; set; }

        public Dictionary<string, EnumerationValue> Values { get; set; }

        public EnumerationType(string name)
        {
            this.Name = name;
            this.Values = new Dictionary<string, EnumerationValue>();
        }
    }
}
