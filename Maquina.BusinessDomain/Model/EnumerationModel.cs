using System.Collections.Generic;

namespace Maquina.BusinessDomain.RulesEngine.Model
{
    public class EnumerationModel
    {
        public string Name { get; }

        public List<EnumerationValueModel> Values { get; }

        public EnumerationModel(string name)
        {
            this.Name = name;
            this.Values = new List<EnumerationValueModel>();
        }
    }
}
