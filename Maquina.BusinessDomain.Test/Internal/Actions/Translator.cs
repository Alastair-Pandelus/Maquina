using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;

namespace Maquina.Service.Test.RulesEngine.Internal.Actions
{
    // Sample actions
    // Builds https://en.wikipedia.org/wiki/Veni,_vidi,_vici for testing
    [ScriptClass("Caesar")]
    internal class Translator : ScriptClass
    {
        [ScriptMethod(ScriptEntityType.Action, "Come")]
        public bool Veni()
        {
            TestResult.Value = nameof(Veni);

            return true;
        }

        [ScriptMethod(ScriptEntityType.Action, "See")]

        public bool Vidi()
        {
            TestResult.Value += $", {nameof(Vidi)}";

            return true;
        }

        [ScriptMethod(ScriptEntityType.Action, "Conquer")]
        public bool Vici()
        {
            TestResult.Value += $", {nameof(Vici)}";

            return true;
        }

        [ScriptMethod(ScriptEntityType.Action, "Echo")]
        public bool Echo(string value)
        {
            TestResult.Value += $", {value}";

            return true;
        }
    }
}
