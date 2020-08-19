using NUnit.Framework;

namespace Maquina.Service.Test.RulesEngine.Internal.RunTime
{
    [TestFixture]
    public class ContextTest : BaseRulesTest
    {
        [Test]
        public void DependencyInjectionTest()
        {
            Assert.IsNotNull(RulesEngineService);

            RulesEngineService.Evaluate("Context.Condition()=>Context.Action()");
        }
    }
}
