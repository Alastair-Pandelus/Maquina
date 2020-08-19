using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression;
using Maquina.Service.Test.RulesEngine.Internal.Actions;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maquina.Service.Test.RulesEngine.Internal.RunTime
{
    [TestFixture]
    public class ActionsTest : BaseRulesTest
    {
        static readonly string altCallBrackets = $"{Syntax.AltOpeningBracket}{Syntax.AltClosingBracket}";
        static readonly string TrueScriptFunction = $"Logic.True{altCallBrackets}";

        [Test]
        public void Null_Condition_With_Null_Actions_Test()
        {
            Assert.IsTrue(RulesEngineService.Evaluate($"_=>()").Result);
        }

        [Test]
        public void Null_Condition_With_Actions_Test()
        {
            Assert.IsTrue(RulesEngineService.Evaluate($"_=>Caesar.Come();Caesar.See();Caesar.Conquer()").Result);
        }

        [Test]
        public async Task ActionsRun()
        {
            // Arrange
            TestResult.Value = string.Empty;

            // Act
            Assert.IsTrue(await RulesEngineService.Evaluate($"{TrueScriptFunction}=>Caesar.Come();Caesar.See();Caesar.Conquer()"));

            // Assert
            Assert.AreEqual(TestResult.Value, "Veni, Vidi, Vici");
        }

        [Test]
        public async Task ActionsRunWithParameters()
        {
            // Arrange
            TestResult.Value = string.Empty;

            // Act
            Assert.IsTrue(await RulesEngineService.Evaluate($"{TrueScriptFunction}=>Caesar.Come();Caesar.Echo(value:String:\"See\");Caesar.Echo(value:String:\"Conquer\")"));

            // Assert
            Assert.AreEqual(TestResult.Value, "Veni, See, Conquer");
        }
    }
}