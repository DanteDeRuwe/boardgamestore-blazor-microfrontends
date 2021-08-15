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
        }
    }
}
