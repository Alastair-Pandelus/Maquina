using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration;
using System;
using System.Collections.Generic;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Model;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    internal class EnumerationRegistry : Dictionary<string, EnumerationType>
    {
        // The containing object is not suitable for serialisation to the client
        // Create a simplified serialised Model form for this
        public List<EnumerationModel> Model
        {
            get
            {
                List<EnumerationModel> enumerationModels = new List<EnumerationModel>();

                foreach (KeyValuePair<string, EnumerationType> enumerationEntry in this)
                {
                    EnumerationModel enumerationModel = new EnumerationModel(enumerationEntry.Key);

                    foreach(KeyValuePair<string, EnumerationValue> enumerationValue in enumerationEntry.Value.Values)
                    {
                        enumerationModel.Values.Add(new EnumerationValueModel(enumerationValue.Key));
                    }

                    enumerationModels.Add(enumerationModel);
                }

                return enumerationModels;
            }
        }

        public EnumerationValue Parse(string enumerationText)
        {
            string[] typeAndValue = enumerationText.Split(Syntax.EnumerationSeparator);

            if (typeAndValue.Length != 2)
            {
                throw new Exception($"Invalid enumeration format {enumerationText} - {nameof(EnumerationRegistry)}.{nameof(Parse)}");
            };

            string enumerationTypeName = typeAndValue[0];
            string enumerationTypeValue = typeAndValue[1];

            EnumerationType enumerationType;

            if (!this.TryGetValue(enumerationTypeName, out enumerationType))
            {
                throw new Exception($"Missing Enumeration Type {enumerationText}");
            }

            EnumerationValue enumerationValue;

            if( !enumerationType.Values.TryGetValue(enumerationTypeValue, out enumerationValue))
            {
                throw new Exception($"Unknown Enumeration Value {enumerationText}");
            }

            return enumerationValue;
        }
    }
}
