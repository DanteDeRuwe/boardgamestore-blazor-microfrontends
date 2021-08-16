using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using BoardgameStore.Client.Routing;
using BoardgameStore.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Client
{
    internal static class Extensions
    {
        internal static async Task AddMicrofrontendsAsync(this IServiceCollection services, HttpClient client)
        {
            services.AddScoped<RouteManager>();
            
            // TODO remove hardcode
            var dlls = new[]
            {
                await client.GetStreamAsync("/CDN/BoardgameStore.Discover.dll"),
            };

            var assemblies = dlls.Select(AssemblyLoadContext.Default.LoadFromStream).ToList();
            assemblies.Add(Assembly.GetAssembly(typeof(App)));
            var componentCollection = ComponentCollection.FromAssemblies(assemblies);
            services.AddScoped<ComponentCollection>(_=> componentCollection);
            
            services.AddMicrofrontendsDependencyInjection(assemblies);
        }

        private static void AddMicrofrontendsDependencyInjection(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var configure = assembly
                    .GetTypes()
                    .FirstOrDefault(x => string.Equals(x.Name, "Microfrontend", StringComparison.Ordinal))
                    ?.GetMethod("ConfigureServices", BindingFlags.Public | BindingFlags.Static, null,
                        new[] { typeof(IServiceCollection) }, null);

                configure?.Invoke(null, new[] { services });
            }
        }
    }
}
