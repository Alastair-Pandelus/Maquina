using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter
{
    // Subset of CLR-compliant value types supported
    // No other types are supported

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ScriptType
    {
        Int32,    
        Double,
        Boolean,
        String,
        Enum
    }

    internal class ScriptTypes
    {
        public static HashSet<Type> Underlying { get; }

        static ScriptTypes()
        {
            Underlying = new HashSet<Type> { typeof(EnumerationValue) };

            Enum.GetValues(typeof(ScriptType))
                    .Cast<ScriptType>()
                    .ToList()
                    .ForEach(parameterType =>
                    {
                        Underlying.Add(Type.GetType($"{nameof(System)}.{parameterType}"));
                    }
                );
        }
    }
}
