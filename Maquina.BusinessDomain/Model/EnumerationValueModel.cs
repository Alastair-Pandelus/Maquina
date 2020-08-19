namespace Maquina.BusinessDomain.RulesEngine.Model
{
    public class EnumerationValueModel
    {
        public string Name { get; set; }

        public EnumerationValueModel(string name)
        {
            this.Name = name;
        }
    }
}
