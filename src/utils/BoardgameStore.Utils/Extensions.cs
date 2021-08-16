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
        public static void AddMicrofrontends(this IServiceCollection services, IList<Assembly> assemblies)
        {
            // Add the route manager for pages
            services.AddScoped<RouteManager>();
            services.AddScoped<ComponentCollection>(_ => ComponentCollection.FromAssemblies(assemblies));
            services.AddMicrofrontendsDependencyInjection(assemblies);
        }

        private static void AddMicrofrontendsDependencyInjection(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
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
}
