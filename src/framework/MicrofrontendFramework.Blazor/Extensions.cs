using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MicrofrontendFramework.Blazor
{
    public static class Extensions
    {
        /// <summary> Registers a list of microfrontend assemblies into a service collection, taking care of each microfrontend's DI</summary>
        /// <param name="services">The service collection to register microfrontends onto</param>
        /// <param name="assemblies">The microfrontend assemblies</param>
        public static void AddMicrofrontends(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddScoped<AssemblyCollection>(_ => new AssemblyCollection(assemblies));
            
            var componentCollection = new ComponentCollection();
            foreach (var assembly in assemblies)
            {
                componentCollection.AddRange(ComponentCollection.FromAssembly(assembly));
                services.ConfigureMicrofrontend(assembly);
            }

            services.AddScoped<ComponentCollection>(_ => componentCollection);
            services.AddScoped<FragmentMap>(_ => FragmentMap.FromComponents(componentCollection));
        }

        private static void ConfigureMicrofrontend(this IServiceCollection services, Assembly assembly)
        {
            var configureServicesMethod = assembly
                .GetTypes()
                .FirstOrDefault(x => x.Name.Equals("Microfrontend", StringComparison.Ordinal))
                ?.GetMethod("ConfigureServices", BindingFlags.Public | BindingFlags.Static, null,
                    new[] { typeof(IServiceCollection) }, null);

            configureServicesMethod?.Invoke(null, new object[] { services });
        }
    }
}
