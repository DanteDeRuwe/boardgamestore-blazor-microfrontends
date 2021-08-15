using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using BoardgameStore.Client.Routing;
using BoardgameStore.Utils;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            await ConfigureServicesAsync(builder);
            var host = builder.Build();
            await host.RunAsync();
        }

        private static async Task ConfigureServicesAsync(WebAssemblyHostBuilder builder)
        {
            var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            builder.Services.AddScoped<HttpClient>(_ => client);
            
            builder.Services.AddScoped<RouteManager>();
            
            // TODO remove hardcode
            var dlls = new[]
            {
                await client.GetStreamAsync("/CDN/BoardgameStore.Discover.dll"),
            };

            var assemblies = dlls.Select(AssemblyLoadContext.Default.LoadFromStream).ToList();
            assemblies.Add(Assembly.GetAssembly(typeof(App))); //add app shell client assembly too for custom router
            
            var componentCollection = ComponentCollection.FromAssemblies(assemblies);
            builder.Services.AddScoped<ComponentCollection>(_=> componentCollection);
        }
        
    }
}
