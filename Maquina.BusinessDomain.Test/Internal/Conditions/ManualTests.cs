using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;

namespace Maquina.Service.Test.RulesEngine.Internal.Conditions
{
    /// <summary>
    /// Set of script methods that don't follow the convention that
    ///     + Method has bool return 
    ///     + Method has parameters of *only* the types Int32, Double, String and Boolean
    ///     
    /// These test should be run be enabling the condition to be tested and instantiating the rules engine
    /// This should generate an exception when the rules are being loaded
    /// </summary>
    [ScriptClass("Manual")]
    public class ManualTests : ScriptClass
    {
        /* Uncomment for manual test that non-bool methods cause reflection exception on load  */
        /*[ScriptMethod(ScripeEntityType.Condition, nameof(NotValue))]
        public int NotValue()
        {
            return 0;
        }*/

        /*[ScriptMethod(ScripeEntityType.Condition, nameof(NotValidParameterType))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Manual test of incorrect parameter")]
        public bool NotValidParameterType(float shouldBeDouble)
        {
            return true;
        }*/
    }
}
