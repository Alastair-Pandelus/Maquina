using System.Collections.Generic;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public class RulesEngineContextService : IRulesEngineContextService
    {
        Dictionary<string, object> keyValuePairs;

        public RulesEngineContextService()
        {
            keyValuePairs = new Dictionary<string, object>();
        }

        public void Clear()
        {
            keyValuePairs.Clear();
        }

        public object Get(string key)
        {
            return keyValuePairs[key];
        }

        public void Set(string key, object value)
        {
            keyValuePairs[key] = value;
        }
    }
}
