using Maquina.BusinessDomain.RulesEngine.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration
{
    public interface IEnumerationTypeFactory
    {
        public Task<List<EnumerationType>> CreateEnumerationTypes();
    }
}
