using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MicrofrontendFramework.Blazor;

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

            var assemblies = await LoadAssembliesAsync(client, builder.HostEnvironment.IsDevelopment());
            builder.Services.AddMicrofrontends(assemblies); //the magic
        }

        private static async Task<IEnumerable<Assembly>> LoadAssembliesAsync(HttpClient client, bool isDevelopment = true)
        {
            var filePaths = await client.GetFromJsonAsync<string[]>($"/api/assemblies");
            var dllPaths = filePaths?.Where(r => r.EndsWith(".dll"));

            var clientAssembly = Assembly.GetAssembly(typeof(App));
            var assemblies = new List<Assembly> { clientAssembly };

            if (dllPaths is null) return assemblies;

            foreach (var dllPath in dllPaths)
            {
                var pdbPath = Regex.Replace(dllPath, @"\.dll$", ".pdb");
                var pdbShouldBeLoaded = isDevelopment && filePaths.Contains(pdbPath);

                await using var dllStream = await client.GetStreamAsync(dllPath);
                await using var pdbStream = pdbShouldBeLoaded ? await client.GetStreamAsync(pdbPath) : null;

                var assembly = AssemblyLoadContext.Default.LoadFromStream(dllStream, pdbStream);
                assemblies.Add(assembly);
            }
            
            return assemblies;
        }
    }
}
