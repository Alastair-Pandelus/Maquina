using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Maquina.Service.Test.RulesEngine.Internal.RunTime
{
    [TestFixture]
    class EnumerationsTest : BaseRulesTest
    {
        [Test]
        public async Task EnumParameters_Success()
        {
            // Check red + yellow = orange
            Assert.IsTrue(await RulesEngineService.Evaluate($"Colour.Mix(colour1:Enum:Primary.Red,colour2:Enum:Primary.Yellow,mix:Enum:Secondary.Orange)=>Colour.Output(mix:Enum:Secondary.Orange)"));
        }

        [Test]
        public async Task EnumParameters_Wrong_Condition_Enumeration_Type()
        {
            try
            {
                // Secondary.Yellow should be Primary.Yellow
                Assert.IsTrue(await RulesEngineService.Evaluate($"Colour.Mix(colour1:Enum:Primary.Red,colour2:Enum:Secondary.Yellow,mix:Enum:Secondary.Orange)=>Colour.Output(mix:Enum:Secondary.Orange)"));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Parsing Error");
                Assert.AreEqual(e.InnerException.Message, "Enumeration type mismatch colour2:Enum:Secondary.Yellow - Secondary != Primary");
                return;
            }

            // Should be an exception 
            Assert.Fail();
        }

        [Test]
        public async Task EnumParameters_Wrong_Action_Enumeration_Type()
        {
            try
            {
                // Primary.Orange should be Secondary.Yellow
                Assert.IsTrue(await RulesEngineService.Evaluate($"Colour.Mix(colour1:Enum:Primary.Red,colour2:Enum:Primary.Yellow,mix:Enum:Secondary.Orange)=>Colour.Output(mix:Enum:Primary.Orange)"));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Parsing Error");
                Assert.AreEqual(e.InnerException.Message, "Enumeration type mismatch mix:Enum:Primary.Orange - Primary != Secondary");
                return;
            }

            // Should be an exception 
            Assert.Fail();
        }

        [Test]
        public async Task EnumParameters_Invalid_Enumeration()
        {
            try
            {
                // Colour.Red should be Primary.Red
                Assert.IsTrue(await RulesEngineService.Evaluate($"Colour.Mix(colour1:Enum:Colour.Red,colour2:Enum:Primary.Yellow,mix:Enum:Secondary.Orange)=>Colour.Output(mix:Enum:Secondary.Orange)"));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Parsing Error");
                Assert.AreEqual(e.InnerException.Message, "Enumeration type mismatch colour1:Enum:Colour.Red - Colour != Primary");
                return;
            }

            // Should be an exception 
            Assert.Fail();
        }
    }
}
