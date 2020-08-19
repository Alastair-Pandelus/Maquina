using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Enumeration;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Method;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using Maquina.RulesEngine.BusinessDomain.DomainSpecificLanguage.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public class Metamodel 
    {
        internal MethodRegistry Conditions { get; set; }

        internal MethodRegistry Actions { get; set; }

        internal EnumerationRegistry Enumerations { get; set; }

        public Metamodel(IServiceProvider serviceProvider)
        {
            Conditions = new MethodRegistry();
            Actions = new MethodRegistry();
            Enumerations = new EnumerationRegistry();

            // Reflect on the code and pick up all meta model elements
            var assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                // A bit hacky - example only
                .Where(a => a.FullName.Contains("Maquina") || a.FullName.Contains("Legatto"))
                .ToList();

            foreach(Assembly assembly in assemblies)
            {
                Type[] assemblyTypes = assembly.GetTypes();

                LoadReflectedMethods(assemblyTypes);
                LoadReflectedEnumerations(assemblyTypes, serviceProvider).Wait();
            };
        }

        private void LoadReflectedMethods(Type[] assemblyTypes)
        {
            assemblyTypes
                .Where(t => t.IsSubclassOf(typeof(ScriptClass)) && t.GetCustomAttributes(typeof(ScriptClassAttribute), false).Any())
                .ToList()
                .ForEach(@class =>
                {
                    ScriptClassAttribute scriptClass = (ScriptClassAttribute)Attribute.GetCustomAttribute(@class, typeof(ScriptClassAttribute));

                    @class
                        .GetMethods()
                        .Where(m => m.GetCustomAttributes(typeof(ScriptMethodAttribute), false).Any())
                        .ToList()
                        .ForEach(method =>
                        {
                            // throws exception on non-compliant script methods
                            VerifyMethod(method);

                            ScriptMethodAttribute scriptMethod = (ScriptMethodAttribute)Attribute.GetCustomAttribute(method, typeof(ScriptMethodAttribute));
                            string methodName = $"{new MethodNameExpression(scriptClass.Name, scriptMethod.Name)}";

                            EnumParameterTypeMapping enumParameterTypes = GetEnumParameterTypes(method);

                            MethodMapping methodMapping = new MethodMapping(@class, method, enumParameterTypes);

                            // Every method should have a unique name for the script language (the registry should ensure this)
                            switch (scriptMethod.Type)
                            {
                                case ScriptEntityType.Condition:
                                    if (!this.Conditions.ContainsKey(scriptMethod.Name))
                                    {
                                        this.Conditions[methodName] = methodMapping;
                                    }
                                    else
                                    {
                                        throw new Exception($"Duplicate Condition Name {scriptMethod.Type}/{scriptMethod.Name}");
                                    }
                                    break;

                                case ScriptEntityType.Action:
                                    if (!this.Actions.ContainsKey(scriptMethod.Name))
                                    {
                                        this.Actions[methodName] = methodMapping;
                                    }
                                    else
                                    {
                                        throw new Exception($"Duplicate Action Name {scriptMethod.Type}/{scriptMethod.Name}");
                                    }
                                    break;

                                default:
                                    throw new Exception($"Unknown {nameof(ScriptEntityType)} - {nameof(LoadReflectedMethods)}");
                            }
                        });
                });
        }

        private static EnumParameterTypeMapping GetEnumParameterTypes(MethodInfo method)
        {
            EnumParameterTypeMapping enumParameterTypes = new EnumParameterTypeMapping();

            List<ParameterInfo> enumParameters 
                = method
                    .GetParameters()
                    .Where(parameter => parameter.ParameterType == typeof(EnumerationValue))
                    .ToList();

            foreach (var enumParameter in enumParameters)
            {
                ScriptEnumParameterAttribute enumParameterAttribute = (ScriptEnumParameterAttribute)enumParameter.GetCustomAttribute(typeof(ScriptEnumParameterAttribute));

                if(enumParameterAttribute == null)
                {
                    throw new Exception($"Missing {nameof(ScriptEnumParameterAttribute)}, {method.Name} method, {enumParameter.Name} parameter");
                }

                enumParameterTypes[enumParameter.Name] = enumParameterAttribute.EnumTypeName;
            }

            return enumParameterTypes;
        }

        private async Task LoadReflectedEnumerations(Type[] assemblyTypes, IServiceProvider serviceProvider)
        {
            var enumerationFactoryTypes = assemblyTypes
                .Where(t => typeof(IEnumerationTypeFactory).IsAssignableFrom(t) && !t.IsInterface)
                .ToList();

            foreach(var enumerationFactoryType in enumerationFactoryTypes)
            {
                IEnumerationTypeFactory enumerationFactory = ObjectFactory.Create(enumerationFactoryType, serviceProvider) as IEnumerationTypeFactory;

                var enumerationTypes = await enumerationFactory.CreateEnumerationTypes();

                foreach (EnumerationType enumerationType in enumerationTypes)
                {
                    this.Enumerations[enumerationType.Name] = enumerationType;
                }
            };
        }

        /// <summary>
        /// Gets a 'code as data' method
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scriptMethod">Name of condition or action in the form "class.method"</param>
        /// <returns>Method + metadata</returns>
        public MethodMapping GetMethod(ScriptEntityType type, string scriptMethod)
        {
            MethodRegistry methods;

            switch (type)
            {
                case ScriptEntityType.Action:
                    methods = Actions;
                    break;

                case ScriptEntityType.Condition:
                    methods = Conditions;
                    break;

                default:
                    throw new Exception($"Unknown {nameof(ScriptClass)} - {nameof(GetMethod)}");

            }

            MethodMapping mapping;

            if (!methods.TryGetValue(scriptMethod, out mapping))
            {
                throw new Exception($"Missing {type} {scriptMethod}");
            }

            return mapping;
        }

        /// <summary>
        /// Verifies the script method is compliant
        ///     + Has boolean return type
        ///     + Parameters in the allowed value types (Int32, String, Double, Boolean or Enumeration)
        /// </summary>
        /// <param name="method"></param>
        private void VerifyMethod(MethodInfo method)
        {
            var returnType = method.ReturnType;

            if (returnType != typeof(bool) && returnType != typeof(Task<bool>))
            {
                throw new ApplicationException($"{nameof(VerifyMethod)} failure - non Boolean return type = {method.ReturnType}");
            }

            method.GetParameters().ToList().ForEach(parameter =>
            {
                if (!ScriptTypes.Underlying.Contains(parameter.ParameterType))
                {
                    throw new Exception($"Unsupported {nameof(ScriptType)} {method.DeclaringType.Name}.{method.Name} {parameter.ParameterType}");
                }
            });
        }

        internal EnumerationValue ParseEnumeration(string parameterValueText)
        {
            return this.Enumerations.Parse(parameterValueText);
        }
    }
}
