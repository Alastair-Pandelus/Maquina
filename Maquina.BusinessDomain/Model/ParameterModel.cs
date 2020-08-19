using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Maquina.BusinessDomain.RulesEngine.Model
{
    public class ParameterModel
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScriptType Type { get; set; }

        // Only if ScriptType = Enum, need to know which enum type
        public string EnumTypeName { get; set; }
    }
}
