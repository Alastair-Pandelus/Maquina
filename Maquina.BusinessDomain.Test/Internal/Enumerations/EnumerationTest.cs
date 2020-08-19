using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using Maquina.RulesEngine.BusinessDomain.DomainSpecificLanguage.Method;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maquina.Service.Test.RulesEngine.Internal.Enumerations
{
    public class ColourTypesFactory : IEnumerationTypeFactory
    {
        internal const string PrimaryColour = "Primary";
        internal const string SecondaryColour = "Secondary";

        public async Task<List<EnumerationType>> CreateEnumerationTypes()
        {
            await Task.Delay(0);

            EnumerationType primaryColour = new EnumerationType(PrimaryColour);
            primaryColour.Values["Red"] = new EnumerationValue { Type = primaryColour, Id = 1, Value = "Red" };
            primaryColour.Values["Yellow"] = new EnumerationValue { Type = primaryColour, Id = 2, Value = "Yellow" };

            EnumerationType secondaryColour = new EnumerationType(SecondaryColour);
            secondaryColour.Values["Orange"] = new EnumerationValue { Type = secondaryColour, Id = 1, Value = "Orange" };
            secondaryColour.Values["Purple"] = new EnumerationValue { Type = secondaryColour, Id = 2, Value = "Purple" };

            return new List<EnumerationType>
            {
                primaryColour,
                secondaryColour
            };
        }
    }

    [ScriptClass("Colour")]
    public class EnumerationTest : ScriptClass
    {
        [ScriptMethod(ScriptEntityType.Condition, nameof(Mix))]
        public bool Mix
            (
                [ScriptEnumParameter(ColourTypesFactory.PrimaryColour)] EnumerationValue colour1,
                [ScriptEnumParameter(ColourTypesFactory.PrimaryColour)] EnumerationValue colour2,
                [ScriptEnumParameter(ColourTypesFactory.SecondaryColour)] EnumerationValue mix
            )
        {
            if ($"{colour1}" == "Primary.Red" && $"{colour2}" == "Primary.Yellow")
            {
                return $"{mix}" == "Secondary.Orange";
            }

            return false;
        }

        [ScriptMethod(ScriptEntityType.Action, nameof(Output))]
        public bool Output
            (
                [ScriptEnumParameter(ColourTypesFactory.SecondaryColour)] EnumerationValue mix
            )
        {
            Console.WriteLine($"{mix}");

            return true;
        }
    }
}
