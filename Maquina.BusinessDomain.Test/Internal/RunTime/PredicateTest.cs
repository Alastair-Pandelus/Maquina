using Maquina.Service.Test.RulesEngine.Internal.RunTime;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Maquina.Service.Test.RulesEngineService.Internal.RunTime
{
    [TestFixture]
    public class PredicateTest : BaseRulesTest
    {
        static readonly string TrueScriptFunction = $"Logic.True()";
        static readonly string FalseScriptFunction = $"Logic.False()";

        #region Axioms
        [Test]
        public async Task @True_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"{TrueScriptFunction}=>()"));
        }

        [Test]
        public async Task @False_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"{FalseScriptFunction}=>()"));
        }
        #endregion

        #region Not
        [Test]
        public async Task @NotTrue_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"Not({TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @NotFalse_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Not({FalseScriptFunction})=>()"));
        }
        #endregion 

        #region And
        [Test]
        public async Task @UnaryAndTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"And({TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @UnaryAndFalse_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"And({FalseScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueAndTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"And({TrueScriptFunction},{TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueAndFalse_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"And({TrueScriptFunction},{FalseScriptFunction})=>()"));
        }

        [Test]
        public async Task @FalseAndTrue_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"And({FalseScriptFunction},{TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueAndTrueAndTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"And({TrueScriptFunction},{TrueScriptFunction},{TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueAndTrueAndFalse_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"And({TrueScriptFunction},{TrueScriptFunction},{FalseScriptFunction})=>()"));
        }

        #endregion 

        #region Or
        [Test]
        public async Task @UnaryOrTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or({TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @UnaryOrFalse_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"Or({FalseScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueOrTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or({TrueScriptFunction},{TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueOrFalse_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or({TrueScriptFunction},{FalseScriptFunction})=>()"));
        }

        [Test]
        public async Task @FalseOrTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or({FalseScriptFunction},{TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @FalseOrFalse_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"Or({FalseScriptFunction},{FalseScriptFunction})=>()"));
        }

        [Test]
        public async Task @TrueOrTrueOrTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or({TrueScriptFunction},{TrueScriptFunction},{TrueScriptFunction})=>()"));
        }

        [Test]
        public async Task @FalseOrFalseOrTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or({FalseScriptFunction},{FalseScriptFunction},{TrueScriptFunction})=>()"));
        }

        #endregion

        #region Nested Not
        [Test]
        public async Task @NotNotTrue_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Not(Not({TrueScriptFunction}))=>()"));
        }

        [Test]
        public async Task @NotNotFalse_Equals_False()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Not(Not({TrueScriptFunction}))=>()"));
        }

        [Test]
        public async Task @NotNotNotTrue_Equals_False()
        {
            Assert.IsFalse(await RulesEngineService.Evaluate($"Not(Not(Not({TrueScriptFunction})))=>()"));
        }

        [Test]
        public async Task @NotNotNotFalse_Equals_True()
        {
            Assert.IsTrue(await RulesEngineService.Evaluate($"Not(Not(Not({FalseScriptFunction})))=>()"));
        }
        #endregion

        #region Nested And / Or / Not
        [Test]
        public async Task NestedAndOrNot()
        {
            // Vary last parameter to switch logic
            Assert.IsTrue(await RulesEngineService.Evaluate($"And(Or({FalseScriptFunction},{TrueScriptFunction}),Or(Not({TrueScriptFunction}),{TrueScriptFunction}))=>()"));
            Assert.IsFalse(await RulesEngineService.Evaluate($"And(Or({FalseScriptFunction},{TrueScriptFunction}),Or(Not({TrueScriptFunction}),{FalseScriptFunction}))=>()"));
        }

        [Test]
        public async Task NestedOrAndNot()
        {
            // Vary last parameter to switch logic
            Assert.IsTrue(await RulesEngineService.Evaluate($"Or(And({FalseScriptFunction},{FalseScriptFunction}),And(Not({FalseScriptFunction}),{TrueScriptFunction}))=>()"));
            Assert.IsFalse(await RulesEngineService.Evaluate($"Or(And({FalseScriptFunction},{FalseScriptFunction}),And(Not({FalseScriptFunction}),{FalseScriptFunction}))=>()"));
        }

        #endregion
    }
}