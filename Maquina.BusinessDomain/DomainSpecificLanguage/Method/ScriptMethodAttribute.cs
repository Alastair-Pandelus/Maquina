using System;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ScriptMethodAttribute : Attribute
    {
        public ScriptMethodAttribute(ScriptEntityType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public ScriptEntityType Type { get; }

        public string Name { get;  }
    }
}
