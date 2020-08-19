using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using System;
using System.Linq;
using System.Reflection;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public class MethodMapping
    {
        public ScriptClassAttribute ClassMetadata { get; set; }

        public ScriptMethodAttribute MethodMetadata { get; set; }

        public Type Class { get; set; }

        public MethodInfo Method { get; set; }

        // The build conditions and actions all have an implemented Enum parameter type of EnumerationValue
        // This is a mapper to an underlying type, this property holds the mapping
        public EnumParameterTypeMapping EnumParameterTypes { get; set; }

        public MethodNameExpression ScriptName => new MethodNameExpression(ClassMetadata.Name, MethodMetadata.Name);

        public MethodMapping(Type @class, MethodInfo method, EnumParameterTypeMapping enumParameterTypes)
        {
            this.Class = @class;
            this.Method = method;
            this.EnumParameterTypes = enumParameterTypes;

            this.ClassMetadata = @class.GetCustomAttributes(typeof(ScriptClassAttribute), false).First() as ScriptClassAttribute;
            this.MethodMetadata = method.GetCustomAttributes(typeof(ScriptMethodAttribute), false).First() as ScriptMethodAttribute;
        }
    }
}
