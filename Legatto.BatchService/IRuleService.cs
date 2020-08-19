using Legatto.CoreDomain;
using System;

namespace Legatto.BatchService
{
    public interface IRuleService
    {
        public void AddRule(string rule);

        public void Process(System.Collections.Generic.List<Account> bulkAccounts);
    }
}
