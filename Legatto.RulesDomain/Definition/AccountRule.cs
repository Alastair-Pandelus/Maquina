using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using Maquina.BusinessDomain.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Legatto.CoreDomain;

namespace Maquina.Service.Rules.Definition
{
    [ScriptClass("Account")]
    public class AccountRule : ScriptClass
    {
        protected Account Account => (Account)Context.Get(nameof(Account));
        protected User User => (User)Context.Get(nameof(User));

        public AccountRule()
        {
        }

        [ScriptMethod(ScriptEntityType.Condition, nameof(IsEmpty))]
        public Boolean IsEmpty()
        {
            return Account.GetFileCount(User) == 0;
        }

        [ScriptMethod(ScriptEntityType.Condition, nameof(HasFileCount))]
        public Boolean HasFileCount(int fileCount)
        {
            return Account.GetFileCount(User) > fileCount;
        }

        [ScriptMethod(ScriptEntityType.Condition, nameof(HasAccreditedParties))]
        public Boolean HasAccreditedParties()
        {
            return Account.AccreditedParties.Count > 0;
        }
    }
}
