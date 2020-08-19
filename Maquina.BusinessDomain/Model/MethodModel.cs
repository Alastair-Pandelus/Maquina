using System.Collections.Generic;

namespace Maquina.BusinessDomain.RulesEngine.Model
{
    public class ScriptTypeModel
    {
        public int Id { get; }

        public string Name { get; }

        public ScriptTypeModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
