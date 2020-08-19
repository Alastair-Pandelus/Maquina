using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public abstract class TriggerExpression : Expression
    {
        public static TriggerExpression Parse(Metamodel metaModel, string expressionText)
        {
            try
            {
                if (FullMatch(Syntax.EmptyCondition, expressionText) != null)
                {
                    return null;
                }

                if (FullMatch(Syntax.Condition, expressionText) != null)
                {
                    return ConditionExpression.Parse(metaModel, expressionText);
                }

                if (FullMatch(Syntax.Unary, expressionText) != null)
                {
                    return UnaryExpression.Parse(metaModel, expressionText);
                }

                if (FullMatch(Syntax.Nary, expressionText) != null)
                {
                    return NaryExpression.Parse(metaModel, expressionText);
                }

                throw new Exception($"Unmatched {nameof(TriggerExpression)} {expressionText}");
            }
            catch(Exception e)
            {
                throw new ApplicationException("Parsing Error", e);
            }
        }
    }
}
