using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public class ActionsExpression : Expression
    {
        private List<ActionExpression> Actions;

        public ActionsExpression(List<ActionExpression> actions)
        {
            Actions = actions;
        }

        public static ActionsExpression Parse(Metamodel metaModel, string expressionText)
        {
            try
            {
                // Empty actions
                if (FullMatch(Syntax.EmptyAction, expressionText) != null)
                {
                    return new ActionsExpression(new List<ActionExpression>());
                }

                // List of 1 or more Action
                if (FullMatch(Syntax.Actions, expressionText) != null)
                {
                    List<ActionExpression> actionExpressions = new List<ActionExpression>();

                    expressionText.Split(Syntax.ActionSeparator)
                        .ToList()
                        .ForEach(actionExpressionText =>
                    {
                        actionExpressions.Add(ActionExpression.Parse(metaModel, actionExpressionText));
                    });

                    return new ActionsExpression(actionExpressions);
                }

                throw new ApplicationException($"Unmatched {nameof(ActionExpression)}");
            }
            catch (Exception e)
            {
                throw new ApplicationException("Parsing Error", e);
            }
        }

        public override async Task<bool> Evaluate(RulesEngineService engine)
        {
            foreach (var action in Actions)
            {
                bool status = await action.Evaluate(engine);

                if (!status)
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for(int i =0; i < Actions.Count; i++)
            {
                sb.Append($"{Actions[i]}");
                if (i < Actions.Count-1)
                {
                    sb.Append($"{Syntax.ActionSeparator}");
                }
            }

            return sb.ToString();
        }
    }
}
