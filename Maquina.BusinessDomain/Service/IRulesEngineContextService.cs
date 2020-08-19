using System.Collections.Generic;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public interface IRulesEngineContextService 
    {
        public void Clear();

        public object Get(string key);

        public void Set(string key, object value);
    }
}
