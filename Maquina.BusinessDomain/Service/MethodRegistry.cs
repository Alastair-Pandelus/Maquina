using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using Maquina.BusinessDomain.RulesEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using Maquina.BusinessDomain.RulesEngine.Model;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public class MethodRegistry : Dictionary<string, MethodMapping>
    {
        // The containing object is not suitable for serialisation to the client
        // Create a simplified serialised Model form for this
        public List<MethodModel> Model 
        {
            get
            {
                List<MethodModel> methodModels = new List<MethodModel>();

                foreach (KeyValuePair<string, MethodMapping> methodEntry in this)
                {
                    MethodModel methodModel = new MethodModel(methodEntry.Key);

                    foreach (ParameterInfo parameterInfo in methodEntry.Value.Method.GetParameters())
                    {
                        string parameterTypeName = parameterInfo.ParameterType.Name;

                        methodModel.Parameters.Add(new ParameterModel
                        {
                            Name = parameterInfo.Name,
                            Type = parameterTypeName == nameof(EnumerationValue)
                                    ? ScriptType.Enum
                                    : (ScriptType)Enum.Parse(typeof(ScriptType), parameterTypeName),
                            EnumTypeName = parameterTypeName == nameof(EnumerationValue)
                                    ? methodEntry.Value.EnumParameterTypes[parameterInfo.Name]
                                    : null
                        });
                    }

                    methodModels.Add(methodModel);
                }

                return methodModels;
            }
        }
    }
}
