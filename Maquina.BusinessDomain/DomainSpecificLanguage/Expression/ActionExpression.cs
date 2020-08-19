using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public class ActionExpression : Expression
    {
        protected MethodNameExpression MethodName { get; set; }

        protected Parameters Parameters { get; set; }

        public ActionExpression(MethodNameExpression methodName, Parameters parameters = null)
        {
            MethodName = methodName;
            Parameters = parameters;
        }

        public static ActionExpression Parse(Metamodel metaModel, string expressionText)
        {
            Match match = FullMatch(Syntax.Action, expressionText);

            if (match == null)
            {
                throw new Exception($"Unmatched {nameof(ActionExpression)} {expressionText}");
            };

            MethodNameExpression methodNameExpression = MethodNameExpression.Parse(metaModel, match.Groups[1].Value);
            MethodMapping methodMapping = metaModel.Actions[methodNameExpression.ToString()];
            Parameters parameters = Parameters.Parse(metaModel, methodMapping.EnumParameterTypes, match.Groups[5].Value);

            return new ActionExpression(methodNameExpression, parameters);
        }

        public override string ToString()
        {
            return $"{MethodName}{Syntax.AltOpeningBracket}{Syntax.AltClosingBracket}";
        }

        public override async Task<bool> Evaluate(RulesEngineService rulesEngine)
        {
            MethodMapping methodMapping = rulesEngine.MetaModel.GetMethod(ScriptEntityType.Action, MethodName.ToString());
            var @class = methodMapping.Class;

            // Create the object instance
            object @object = rulesEngine.Instantiate(@class);

            // Manually inject the rules Engine context into the object instance
            @class.GetProperty(nameof(ScriptClass.Context)).SetValue(@object, rulesEngine.Context);

            // Generate the method meta-data
            var method = @class.GetMethod(methodMapping.Method.Name);

            List<object> methodParameters = new List<object>();

            method.GetParameters().ToList().ForEach(reflectionParameter =>
            {
                var parameter = Parameters[reflectionParameter.Name];

                if (parameter == null)
                {
                    throw new ApplicationException($"Missing {nameof(Parameter)} {reflectionParameter.Name}");
                }

                methodParameters.Add(parameter.Value);
            });

            // Invoke the method - possibly asynchronously
            bool status = await methodMapping.Method.InvokeAsync(@object, methodParameters.ToArray());

            return status;
        }
    }
}
