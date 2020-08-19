using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Collections.Generic;

namespace Maquina.Service.Test.RulesEngine.Internal.Context
{
    public class RulesTestContext : IRulesEngineContextService
    {
        private Dictionary<string, object> values;

        public RulesTestContext()
        {
            values = new Dictionary<string, object>();
        }

        public void Clear()
        {
            values.Clear();
        }

        public object Get(string key)
        {
            if (!values.ContainsKey(key))
            {
                throw new Exception($"Missing context key {nameof(RulesTestContext)}.{nameof(Get)} - {key}");
            }

            return values[key];
        }

        public void Set(string key, object value)
        {
            values[key] = value;
        }
    }
}
