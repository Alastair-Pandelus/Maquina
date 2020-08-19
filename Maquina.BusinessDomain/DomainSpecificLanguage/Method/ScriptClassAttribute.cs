using System;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScriptClassAttribute : Attribute
    {
        public ScriptClassAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
