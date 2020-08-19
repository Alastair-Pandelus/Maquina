using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public class ConditionExpression : TriggerExpression
    {
        protected MethodNameExpression MethodName { get; set; }

        protected Parameters Parameters { get; set; }

        public ConditionExpression(MethodNameExpression methodNameExpression, Parameters parameters = null)
        {
            MethodName = methodNameExpression;
            Parameters = parameters;
        }

        public new static ConditionExpression Parse(Metamodel metaModel, string expressionText)
        {
            var groupedMatches = Syntax.Condition.Matches(expressionText)[0].Groups;

            // e.g - Func.GreaterThan:Int32:1,Logic.IsTrue:Boolean:True
            string methodNameText = groupedMatches[1].Value;
            string parametersText = groupedMatches[5].Value;

            MethodMapping methodMapping = metaModel.Conditions[methodNameText];

            MethodNameExpression methodNameExpression = MethodNameExpression.Parse(metaModel, methodNameText);
            Parameters parameters = Parameters.Parse(metaModel, methodMapping.EnumParameterTypes, parametersText);

            return new ConditionExpression(methodNameExpression, parameters);
        }

        public override async Task<bool> Evaluate(RulesEngineService rulesEngine)
        {
            var methodMapping = rulesEngine.MetaModel.GetMethod(ScriptEntityType.Condition, MethodName.ToString());
            var @class = methodMapping.Class;

            // Class -> object
            object @object = rulesEngine.Instantiate(@class);

            // Manually inject the rules Engine context into the object instance
            @class.GetProperty(nameof(ScriptClass.Context)).SetValue(@object, rulesEngine.Context);
            
            // Marshall the method
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

            bool status = await method.InvokeAsync(@object, methodParameters.ToArray());

            return status;
        }
        
        public override string ToString()
        {
            var parameters = Parameters?.Count > 0 ? Parameters.ToString() : string.Empty;

            return $"{MethodName}({parameters})";
        }
    }
}
