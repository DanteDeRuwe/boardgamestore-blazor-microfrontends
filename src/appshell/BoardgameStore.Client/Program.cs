using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using BoardgameStore.Utils;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            await Configure(builder);
            await builder.Build().RunAsync();
        }

        private static async Task Configure(WebAssemblyHostBuilder builder)
        {
            var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            builder.Services.AddScoped<HttpClient>(_ => client);
            
            // Get assemblies
            // TODO remove hardcode
            var dlls = new[]
            {
                await client.GetStreamAsync("/CDN/BoardgameStore.Discover.dll"),
            };

            var assemblies = dlls.Select(AssemblyLoadContext.Default.LoadFromStream).ToList();
            assemblies.Add(Assembly.GetAssembly(typeof(App)));
            
            
            builder.Services.AddMicrofrontends(assemblies); //the magic
        }
        
    }
}
