using System;
using System.Collections.Generic;
using System.Linq;
using Maquina.BusinessDomain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Maquina.Service.Test
{
    [TestFixture]
    public abstract class BaseTest
    {
        private static IServiceProvider _serviceProvider;
        protected ServiceCollection _services;

        [SetUp]
        public void BaseSetup()
        {
            _services = new ServiceCollection();

            var configuration = GetConfiguration();

            // Add in test specific ones
            _services.AddSingleton(typeof(IConfiguration), configuration);

            // Pick up a subset of the standard DI mappings required for testing
            TestApiServices.ConfigureServices(_services, configuration);

            // Let the test classes override the default DI dependencies 
            OverrideCustomServices();

            _serviceProvider = _services.BuildServiceProvider();
        }

        // Hook to allow derived tests to override custom services
        public virtual void OverrideCustomServices()
        { }

        private Dictionary<Type, object> injections = new Dictionary<Type, object>();

        /// <summary>
        /// Picks up a dependency injection by Type
        /// </summary>
        /// <typeparam name="T">Requested Interface type</typeparam>
        /// <param name="cache">true if using persisted previous value</param>
        /// <returns>Dependency Injected class derived from passed interface</returns>
        public T GetInjection<T>(bool cache = false)
        {
            object injection;
            Type t = typeof(T);

            if (!t.IsInterface)
            {
                throw new Exception($"Expecting to be injecting interface type - {t.Name}");
            }

            if (!(cache && injections.TryGetValue(t, out injection)))
            {
                injection = _serviceProvider.GetService(t);

                if (cache && injection != null)
                {
                    injections[typeof(T)] = injection;
                }
            }

            if (injection == null)
            {
                throw new Exception($"Missing injection service of type - {t.Name}");
            }

            return (T)injection;
        }

        protected void InjectSingleton<T>(T instance)
        {
            // override any exsiting injection with a test one
            var serviceDescriptor = _services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null)
            {
                _services.Remove(serviceDescriptor);
            }
            _services.AddSingleton(typeof(T), instance);
        }

        protected void InjectTransient(Type I, Type t)
        {
            // override any existing injection with a test one
            ServiceDescriptor serviceDescriptor = _services.FirstOrDefault(descriptor => descriptor.ServiceType == I);
            if (serviceDescriptor != null)
            {
                _services.Remove(serviceDescriptor);
            }
            _services.AddTransient(I, t);
        }

        private static IConfiguration GetConfiguration()
        {
            IConfigurationBuilder configurationBuilder
                = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json" });

            return configurationBuilder.Build();
        }
    }
}
