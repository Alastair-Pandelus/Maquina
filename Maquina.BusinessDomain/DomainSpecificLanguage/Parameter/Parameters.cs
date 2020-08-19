using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Service;
using System.Collections.Generic;
using System.Text;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter
{
    public class Parameters : List<Parameter>
    {
        public Parameter this[string name]
        {
            get { return this.Find(parameter => parameter.Name == name); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i=0; i < Count-1; i++)
            {
                // Parameters are ordered, so names not required
                sb.Append($"{this[i]}{Syntax.ParameterSeparator}");
            };

            sb.Append($"{this[Count-1]}");

            return sb.ToString();
        }

        // e.g. CaseId:Int32:1,IncludeInactive:Boolean:True
        internal static Parameters Parse(Metamodel metaModel, EnumParameterTypeMapping enumParameterTypeMapping, string expressionText)
        {
            Parameters parameters = new Parameters();

            if (!string.IsNullOrWhiteSpace(expressionText))
            {
                Split(expressionText).ForEach(parameterText =>
                {
                    parameters.Add(Parameter.Parse(metaModel, enumParameterTypeMapping, parameterText));
                });
            }

            return parameters;
        }

        /// <summary>
        /// Split by comma, taking care there are none embedded in string parameters
        /// </summary>
        /// <param name="expression">expression to split</param>
        /// <returns></returns>
        private static List<string> Split(string expression)
        {
            List<string> segments = new List<string>();

            int segmentStart = 0;
            bool inString = false;
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    // Start or end of string - assume there are no escaped " chars in the string for now
                    case Syntax.StringQuotation:
                        inString = !inString;
                        break;

                    default:
                        if (!inString && expression[i] == Syntax.ParameterSeparator)
                        {
                            segments.Add(expression[segmentStart..i]);
                            segmentStart = i + 1;
                        }
                        break;
                }
            }

            segments.Add(expression[segmentStart..]);

            return segments;
        }
    }
}
