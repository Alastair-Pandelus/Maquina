using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public class MethodNameExpression : Expression
    {
        public static Regex MethodNameRegularExpression = new Regex($"([a-zA-Z_][a-zA-Z0-9_]*).([a-zA-Z_][a-zA-Z0-9_]*)", RegexOptions.Compiled);

        string ClassName { get; }

        string MethodName { get; set; }

        public MethodNameExpression(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
        }

        public override string ToString()
        {
            return $"{ClassName}{Syntax.ClassMethodSeparator}{MethodName}";
        }

        public override async Task<bool> Evaluate(RulesEngineService engine)
        {
            await Task.Delay(0);

            // Not executable - parent Condition / Action is
            throw new NotImplementedException();
        }

        public static MethodNameExpression Parse(Metamodel metaModel, string expressionText)
        {
            Match methodNameMatch = FullMatch(MethodNameRegularExpression, expressionText);

            if( methodNameMatch == null )
            {
                throw new Exception($"Parsing Error {nameof(MethodNameExpression)} {expressionText}");
            };

            return new MethodNameExpression(methodNameMatch.Groups[1].Value, methodNameMatch.Groups[2].Value);
        }
    }
}
