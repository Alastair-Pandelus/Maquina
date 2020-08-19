using Maquina.BusinessDomain.RulesEngine.Service;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public abstract class Expression 
    {
        public abstract Task<bool> Evaluate(RulesEngineService engine);

        protected static Match FullMatch(Regex regex, string expressionText)
        {
            var match = regex.Match(expressionText);

            return (match.Success && match.Value == expressionText) ? match : null;
        }
    }
}
