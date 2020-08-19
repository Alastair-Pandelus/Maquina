using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter
{
    public class Parameter
    {
        internal string Name { get; }

        internal ScriptType Type { get; }

        internal dynamic Value { get;  }

        public Parameter(string name, ScriptType type, dynamic value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}{Syntax.ParameterQualifier}{Type}{Syntax.ParameterQualifier}{GetValueString()}";
        }

        private string GetValueString()
        {
            return this.Type==ScriptType.String ? $"{Syntax.StringQuotation}{Value}{Syntax.StringQuotation}" : $"{Value}";
        }

        // Handle - e.g Boolean:True
        internal static Parameter Parse(Metamodel metaModel, EnumParameterTypeMapping enumParameterTypeMapping, string parameterText)
        {
            string[] parameterElements = parameterText.Split(Syntax.ParameterQualifier);
            if (parameterElements.Length != 3)
            {
                throw new Exception($"Invalid Name:Type:Value format Parameter Value {parameterText}");
            }

            string name = parameterElements[0];

            ScriptType type;
            if (!Enum.TryParse<ScriptType>(parameterElements[1], out type))
            {
                throw new Exception($"Unknown Parameter Type {parameterElements[1]} - {parameterText}");
            }

            string valueText = parameterElements[2];

            dynamic value = null;

            switch(type)
            {
                case ScriptType.Boolean:
                    bool boolValue;
                    if (!Boolean.TryParse(valueText, out boolValue))
                    {
                        throw new ApplicationException($"Invalid {nameof(Boolean)} value {valueText}");
                    }
                    value = boolValue;
                    break;

                case ScriptType.Int32:
                    int intValue;
                    if (!Int32.TryParse(valueText, out intValue))
                    {
                        throw new ApplicationException($"Invalid {nameof(Int32)} value {valueText}");
                    }
                    value = intValue;
                    break;

                case ScriptType.Double:
                    double doubleValue;
                    if (!Double.TryParse(valueText, out doubleValue))
                    {
                        throw new ApplicationException($"Invalid {nameof(Double)} value {valueText}");
                    }
                    value = doubleValue;
                    break;

                case ScriptType.String:
                    if(valueText.Length < 2 || valueText[0]!=Syntax.StringQuotation || valueText[^1]!=Syntax.StringQuotation)
                    {
                        throw new Exception($"Unquoted string {nameof(Parameter)} {valueText}");
                    }
                    // Remove leading and trailing " chars
                    value = valueText[1..^1];
                    break;

                case ScriptType.Enum:
                    string[] valueSplit = valueText.Split(Syntax.EnumerationSeparator);
                    if(valueSplit.Length != 2)
                    {
                        throw new Exception("Invalid enum value");
                    }
                    string valueEnumType = valueSplit[0];

                    string enumParameterType;
                    if (!enumParameterTypeMapping.TryGetValue(name, out enumParameterType))
                    {
                        throw new Exception($"Missing enumeration parameter type {parameterText}");
                    }

                    if(valueEnumType != enumParameterType)
                    {
                        throw new Exception($"Enumeration type mismatch {parameterText} - {valueEnumType} != {enumParameterType}");
                    }
                    value = metaModel.ParseEnumeration(valueText);
                    break;

                default:
                    throw new Exception("Unexpected Type");
            }

            return new Parameter(name, type, value);
        }
    }
}
