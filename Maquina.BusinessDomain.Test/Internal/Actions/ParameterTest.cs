using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;

namespace Maquina.Service.Test.RulesEngine.Internal.Actions
{
    // Sample Action with one of every type of parameter
    [ScriptClass("Calc")]
    internal class ParameterTest : ScriptClass
    {
        [ScriptMethod(ScriptEntityType.Condition, nameof(IsEven))]
        public bool IsEven(int intValue, double doubleValue, string stringValue, bool booleanValue)
        {
            int value = intValue + (int)doubleValue + int.Parse(stringValue) + (booleanValue ? 1 : 0);

            return value % 2 == 0;
        }
    }
}
