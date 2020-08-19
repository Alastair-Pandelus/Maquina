using System.Collections.Generic;

namespace Maquina.BusinessDomain.RulesEngine.Model
{
    public class MethodModel
    {
        public string Name { get; }

        public List<ParameterModel> Parameters { get; }

        public MethodModel(string name)
        {
            this.Name = name;
            this.Parameters = new List<ParameterModel>();
        }
    }
}
