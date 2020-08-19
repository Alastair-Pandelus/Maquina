using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration
{
    public class EnumerationValue
    {
        public EnumerationType Type { get; set; }

        public int Id { get; set; }

        public string Value { get; set; }

        public EnumerationValue()
        { }

        public EnumerationValue(EnumerationType type, int id, string value)
        {
            this.Type = type;
            this.Id = id;
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{Type.Name}{Syntax.EnumerationSeparator}{Value}";
        }
    }
}
