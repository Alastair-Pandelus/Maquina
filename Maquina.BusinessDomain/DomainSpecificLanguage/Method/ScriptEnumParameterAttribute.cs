using System;

namespace Maquina.RulesEngine.BusinessDomain.DomainSpecificLanguage.Method
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class ScriptEnumParameterAttribute : Attribute
    {
        public ScriptEnumParameterAttribute(string enumTypeName)
        {
            this.EnumTypeName = enumTypeName;
       }

        public string EnumTypeName { get;  }
    }
}
