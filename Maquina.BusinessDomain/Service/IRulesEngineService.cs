using Maquina.BusinessDomain.RulesEngine.Model;
using Maquina.BusinessDomain.Shared;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public interface IRulesEngineService
    {
        public Task<ServiceResponseModel<MetamodelModel>> GetMetamodel();

        public IRulesEngineContextService Context { get; }

        public Task<RuleStatusModel> Validate(string ruleExpression);

        public Task<bool> Evaluate(string ruleExpression);
    }
}
