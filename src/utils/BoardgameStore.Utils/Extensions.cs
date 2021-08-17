using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BoardgameStore.Utils.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Utils
{
    public static class Extensions
    {
        public static void AddMicrofrontends(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            // Add the route manager for pages
            services.AddScoped<RouteManager>();

            var componentCollection = new ComponentCollection();

            foreach (var assembly in assemblies)
            {
                componentCollection.AddRange(ComponentCollection.FromAssembly(assembly));
                services.ConfigureMicrofrontend(assembly);
            }

            services.AddScoped<ComponentCollection>(_ => componentCollection);
            var fragmentMap = FragmentMap.FromComponents(componentCollection);
            services.AddScoped<FragmentMap>(_ => fragmentMap);
        }

        private static void ConfigureMicrofrontend(this IServiceCollection services, Assembly assembly)
        {
            var configureServicesMethod = assembly
                .GetTypes()
                .FirstOrDefault(x => x.Name.Equals("Microfrontend", StringComparison.Ordinal))
                ?.GetMethod("ConfigureServices", BindingFlags.Public | BindingFlags.Static, null,
                    new[] { typeof(IServiceCollection) }, null);

            configureServicesMethod?.Invoke(null, new[] { services });
        }
    }
}
