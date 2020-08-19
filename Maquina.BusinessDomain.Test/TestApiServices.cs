using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maquina.Service.Test
{
    // Integration testing requires a subset of the full application services
    public class TestApiServices
    {
        private static readonly string[] serviceSuffices = { "Service" };

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            Maquina.BusinessDomain.RulesEngine.Service.RulesEngineService rulesEngine = null;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            const string Maquina = "Maquina";
            const string Legatto = "Legatto";

            // Reflect over all assemblies - services and jobs are in multiple projects
            List<Assembly> assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                    .Where(assembly => assembly.FullName.Contains(Maquina) || assembly.FullName.Contains(Legatto))
                    .ToList();

            List<Type> types = assemblies.SelectMany(a => a.GetTypes())
                    .Where(c => c.IsClass &&
                            (c.FullName.Contains(Maquina) || c.FullName.Contains(Legatto)) &&
                            !c.IsAbstract &&
                            (serviceSuffices.Any(suffix => c.Name.EndsWith(suffix))))
                .ToList();

            types.ForEach(t =>
            {
                var baseInterfaceType = t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name);

                if (baseInterfaceType != null)
                {
                    // Map all the IService interfaces -> Service class implementations
                    services.AddTransient(baseInterfaceType, t);
                }
            });
        }
    }
}