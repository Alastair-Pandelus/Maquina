using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Operator;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public class UnaryExpression : TriggerExpression
    {
        UnaryOperator Operator { get; }

        public TriggerExpression Operand { get; set; }

        public UnaryExpression(UnaryOperator @operator, TriggerExpression operand)
        {
            Operator = @operator;
            Operand = operand;
        }

        public new static UnaryExpression Parse(Metamodel metaModel, string expressionText)
        {
            Match match = FullMatch(Syntax.Unary, expressionText);

            if (match == null)
            {
                throw new Exception($"Unmatched {nameof(UnaryExpression)} {expressionText}");
            }

            var matchGroup = match.Groups[1];

            string subExpressionText = matchGroup.Value[1..^1];
            TriggerExpression subExpression = TriggerExpression.Parse(metaModel, subExpressionText);

            return new UnaryExpression(UnaryOperator.Not, subExpression);
        }

        public override string ToString()
        {
            return $"{Operator}({Operand})";
        }

        public override async Task<bool> Evaluate(RulesEngineService rulesEngine)
        {
            switch (Operator)
            {
                case UnaryOperator.Not:
                    bool status = await Operand.Evaluate(rulesEngine);

                    return !status;

                default:
                    throw new Exception($"Unexpected {nameof(UnaryOperator)}");
            }
        }
    }
}
