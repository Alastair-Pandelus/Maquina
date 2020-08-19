using Legatto.CoreDomain;
using Legatto.RulesDomain;
using Maquina.BusinessDomain.RulesEngine.Service;
using System.Collections.Generic;

namespace Legatto.BatchService
{
    public class RuleService : IRuleService
    {
        List<string> rules = new List<string>();

        private IRulesEngineService RulesEngineService { get; }

        public RuleService(IRulesEngineService rulesEngineService)
        {
            this.RulesEngineService = rulesEngineService;
        }

        public void AddRule(string rule)
        {
            rules.Add(rule);
        }

        public void Process(List<Account> bulkAccounts)
        {
            rules.ForEach(rule =>
            {
                bulkAccounts.ForEach(account =>
                {
                    RulesEngineService.Context.Set(nameof(Account), account);

                    account.Users.ForEach(async user =>
                    {
                        RulesEngineService.Context.Set(nameof(User), user);

                        await RulesEngineService.Evaluate(rule);
                    });
                });
            });
        }
    }
}
