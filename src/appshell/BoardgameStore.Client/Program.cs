using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            
            var assemblies = await GetAssembliesAsync(client);
            builder.Services.AddMicrofrontends(assemblies); //the magic
        }

        private static async Task<List<Assembly>> GetAssembliesAsync(HttpClient client)
        {
            var assemblyFiles = await client.GetFromJsonAsync<string[]>("/api/assemblies");
            var assemblies = new List<Assembly>();
            
            if (assemblyFiles is null) return assemblies;
            foreach (var f in assemblyFiles)
            {
                var dll = await client.GetStreamAsync($"{f}");
                assemblies.Add(AssemblyLoadContext.Default.LoadFromStream(dll));
            }

            assemblies.Add(Assembly.GetAssembly(typeof(App)));
            return assemblies;
        }
    }
}
