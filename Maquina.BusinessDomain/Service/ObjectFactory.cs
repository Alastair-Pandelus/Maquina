using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maquina.BusinessDomain.RulesEngine.Service
{
    public class ObjectFactory
    {
        /// <summary>
        /// Creates an object instance of a given type, injecting services as required
        /// </summary>
        /// <param name="class">Type of object</param>
        /// <param name="serviceProvider"></param>
        /// <returns>Object Instance</returns>
        internal static object Create(Type @class, IServiceProvider serviceProvider)
        {
            var constructor = @class.GetConstructors()[0];
            List<object> injectedParameters = new List<object>();

            constructor
                .GetParameters()
                .ToList()
                .ForEach(constructorParameter =>
                {
                    injectedParameters.Add(serviceProvider.GetService(constructorParameter.ParameterType));
                });

            // Safety over speed, see https://stackoverflow.com/questions/2024435/how-to-pass-ctor-args-in-activator-createinstance-or-use-il
            return Activator.CreateInstance(@class, args: injectedParameters.ToArray());
        }
    }
}
