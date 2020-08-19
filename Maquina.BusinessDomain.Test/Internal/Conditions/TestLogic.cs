using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;

namespace Maquina.Service.Test.RulesEngine.Internal.Conditions
{
    [ScriptClass("Logic")]
    public class TestLogic : ScriptClass
    {
        [ScriptMethod(ScriptEntityType.Condition, nameof(True))]
        public bool True()
        {
            return true;
        }

        [ScriptMethod(ScriptEntityType.Condition, nameof(False))]
        public bool False()
        {
            return false;
        }
    }
}
