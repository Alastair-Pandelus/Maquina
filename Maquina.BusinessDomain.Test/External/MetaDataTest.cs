using Maquina.BusinessDomain.RulesEngine;
using Maquina.BusinessDomain.RulesEngine.Model;
using Maquina.BusinessDomain.Shared;
using Maquina.BusinessDomain.RulesEngine.Service;
using Maquina.Service.Test.RulesEngine.Internal.RunTime;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Maquina.Service.Test.RulesEngine.Rules.External
{
    [TestFixture]
    class MetaDataTest : BaseRulesTest
    {
        [Test]
        public void Get_Metamodel_Success()
        {
            #region Arrange

            IRulesEngineService rulesEngineService = GetInjection<IRulesEngineService>();

            ServiceResponseModel<MetamodelModel> metamodel = rulesEngineService.GetMetamodel().Result;

            string output = JsonConvert.SerializeObject(metamodel.Data);
            Assert.IsTrue(!string.IsNullOrEmpty(output));

            #endregion
        }
    }
}
