using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    class RuleExpression : Expression
    {
        private TriggerExpression triggerExpression;
        private ActionsExpression actionsExpression;

        public RuleExpression(TriggerExpression triggerExpression, ActionsExpression actionsExpression)
        {
            this.triggerExpression = triggerExpression;
            this.actionsExpression = actionsExpression;
        }

        internal static object Parse(Metamodel metaModel, string stringExpression)
        {
            if (FullMatch(Syntax.Rule, stringExpression) == null)
            {
                throw new Exception($"Unmatched {nameof(RuleExpression)} {stringExpression}");
            }

            var subClauses = stringExpression.Split(Syntax.Implies);

            TriggerExpression triggerExpression = TriggerExpression.Parse(metaModel, subClauses[0]);
            ActionsExpression actionsExpression = ActionsExpression.Parse(metaModel, subClauses[1]);

            return new RuleExpression(triggerExpression, actionsExpression);
        }

        public override async Task<bool> Evaluate(RulesEngineService engine)
        {
            // It's possible to have a null trigger expression "_=>{Actions}"
            // Make code easy to step through in debugger
            if (triggerExpression != null && !await triggerExpression.Evaluate(engine))
            {
                return false;
            }

            bool status = await actionsExpression.Evaluate(engine);

            return status;
        }
    }
}
