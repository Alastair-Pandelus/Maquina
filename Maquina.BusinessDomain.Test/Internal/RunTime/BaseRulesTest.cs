using Maquina.BusinessDomain.RulesEngine.Service;
using NUnit.Framework;

namespace Maquina.Service.Test.RulesEngine.Internal.RunTime
{
    public class BaseRulesTest : BaseTest
    {
        public IRulesEngineService RulesEngineService { get; set; }

        [SetUp]
        public void Setup()
        {
            RulesEngineService = GetInjection<IRulesEngineService>();
        }
    }
}
