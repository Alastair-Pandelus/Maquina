using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using Maquina.BusinessDomain.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Legatto.CoreDomain;
using Legatto.RulesDomain;

namespace Maquina.Service.Rules.Definition
{
    [ScriptClass("Message")]
    public class MessageRule : ScriptClass
    {
        protected Account Account => (Account)Context.Get(nameof(Account));
        protected User User => (User)Context.Get(nameof(User));

        private IMessageBusService MessageBusService { get; }

        public MessageRule(IMessageBusService messageBusService)
        {
            this.MessageBusService = messageBusService;
        }

        [ScriptMethod(ScriptEntityType.Action, nameof(Publish))]
        public Boolean Publish(String message)
        {
            this.MessageBusService.Publish($"Rule Fired - Account {Account.Id}, User={User.Username} - {message}");

            return true;
        }
    }
}
