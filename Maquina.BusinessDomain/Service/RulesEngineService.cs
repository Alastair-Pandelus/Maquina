using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using Maquina.BusinessDomain.Shared;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.Model;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public class RulesEngineService : IRulesEngineService
    {
        internal Metamodel MetaModel { get; set; }

        public IRulesEngineContextService Context { get; }

        private IServiceProvider ServiceProvider { get; }

        public RulesEngineService(IRulesEngineContextService context, IServiceProvider serviceProvider)
        {
            this.Context = context;
            this.ServiceProvider = serviceProvider;
            this.MetaModel = new Metamodel(serviceProvider);
        }

        public async Task<ServiceResponseModel<MetamodelModel>> GetMetamodel()
        {
            // Keep the async interface, but make it de-facto synchronous
            await Task.Delay(0).ConfigureAwait(true);

            MetamodelModel metamodelModel = new MetamodelModel
            {
                ScriptTypes = this.GetAllScriptTypes(),
                Enumerations = this.MetaModel.Enumerations.Model,
                Conditions = this.MetaModel.Conditions.Model,
                Actions = this.MetaModel.Actions.Model
            };

            return new ServiceResponseModel<MetamodelModel>
            {
                Data = metamodelModel
            };
        }

        private List<ScriptTypeModel> GetAllScriptTypes()
        {
            // Keep the async interface, but make it de-facto synchronous
            List<ScriptTypeModel> scriptTypeModels = new List<ScriptTypeModel>();

            Enum.GetValues(typeof(ScriptType))
                .Cast<ScriptType>()
                .ToList()
                .ForEach(scriptType =>
                {
                    scriptTypeModels.Add(new ScriptTypeModel((int)scriptType, scriptType.ToString()));
                });

            return scriptTypeModels;
        }

        /// <summary>
        /// Checks if a rule expression is valid
        /// </summary>
        /// <param name="ruleExpression">Rules expression</param>
        /// <returns>Status with flag and message giving reason if invalid</returns>
        public async Task<RuleStatusModel> Validate(string ruleExpression)
        {
            // psuedo async
            await Task.Delay(0).ConfigureAwait(true);

            string altStringExpression = GetAltBracketExpression(ruleExpression);

            try
            {
                RuleExpression.Parse(this.MetaModel, altStringExpression);
            }
            catch( Exception e )
            {
                return new RuleStatusModel { IsValid = false, Error = e.Message };
            }

            return new RuleStatusModel { IsValid = true };
        }


        public async Task<bool> Evaluate(string ruleExpression)
        {
            string altStringExpression = GetAltBracketExpression(ruleExpression);

            RuleExpression rule = (RuleExpression)RuleExpression.Parse(this.MetaModel, altStringExpression);

            bool status = await rule.Evaluate(this);

            return status;
        }

        private string GetAltBracketExpression(string stringExpression)
        {
            StringBuilder altStringBuilder = new StringBuilder();

            bool inString = false;

            foreach(char c in stringExpression)
            {
                char altC = c;

                if (c == '"')
                {
                    inString = !inString;
                }
                
                if (!inString)
                { 
                    switch (c)
                    {
                        case Syntax.OpeningBracket:
                            altC = Syntax.AltOpeningBracket;
                            break;

                        case Syntax.ClosingBracket:
                            altC = Syntax.AltClosingBracket;
                            break;
                    }
                }

                altStringBuilder.Append(altC);
            }

            return altStringBuilder.ToString();
        }

        /// <summary>
        /// Creates an object instance of a given type, injecting services as required
        /// </summary>
        /// <param name="class">Type of object</param>
        /// <returns>Object Instance</returns>
        internal object Instantiate(Type @class)
        {
            return ObjectFactory.Create(@class, ServiceProvider);
        }
    }
}
