using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Maquina.Service.Test.RulesEngine.Internal.RunTime
{
    [TestFixture]
    class ParametersTest : BaseRulesTest
    {
        [Test]
        public async Task PassAllParameterTypes()
        {
            // IsEven(int intValue, double doubleValue, string stringValue, bool booleanValue)
            Assert.IsTrue(await RulesEngineService.Evaluate($"Calc.IsEven(intValue:Int32:1,doubleValue:Double:2.0,stringValue:String:\"3\",booleanValue:Boolean:False)=>()"));
            Assert.IsFalse(await RulesEngineService.Evaluate($"Calc.IsEven(intValue:Int32:1,doubleValue:Double:1.0,stringValue:String:\"3\",booleanValue:Boolean:False)=>()"));
        }

        [Test]
        public async Task MissingParameter()
        {
            try
            {
                await RulesEngineService.Evaluate($"Calc.IsEven(intValue:Int32:1,stringValue:String:\"3\",booleanValue:Boolean:False)=>()");
            }
            catch (ApplicationException)
            {
                // Success
                return;
            }

            Assert.Fail("No Exception caught");
        }

        [Test]
        public async Task TypeMismatch()
        {
            try
            {
                await RulesEngineService.Evaluate($"Calc.IsEven(intValue:Int32:1.0,doubleValue:Double:2.0,stringValue:String:\"3\",booleanValue:Boolean:False)=>()");
            }
            catch (ApplicationException)
            {
                // Success
                return;
            }

            Assert.Fail("No Exception caught");
        }
    }
}
