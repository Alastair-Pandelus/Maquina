using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public static class MethodExtension
    {
        // Invokes a method using reflection, takes account of whether the method is asynchronous or not
        public static async Task<bool> InvokeAsync(this MethodInfo method, object @object, params object[] parameters)
        {
            if (method.IsAsynchronous())
            {
                Task<bool> result = (Task<bool>)method.Invoke(@object, parameters);

                bool status = await result;

                return status;
            }
            else
            {
                return (bool)method.Invoke(@object, parameters);
            }
        }

        public static bool IsAsynchronous(this MethodInfo method)
        {
            return method.ReturnType == typeof(Task<bool>);
        }
    }
}
