using BoardgameStore.Utils;
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

            var assemblies = await GetAssembliesAsync(client, builder.HostEnvironment.IsDevelopment());
            builder.Services.AddMicrofrontends(assemblies); //the magic
        }

        private static async Task<List<Assembly>> GetAssembliesAsync(HttpClient client, bool isDevelopment = true)
        {
            var fileNames = await client.GetFromJsonAsync<string[]>($"/api/assemblies");
            
            var assemblies = new List<Assembly>();
            if (fileNames is null) return assemblies;

            var dlls = fileNames.Where(r => r.EndsWith(".dll"));
            foreach (var dll in dlls)
            {
                var pdbPath = Regex.Replace(dll, @"\.dll$", ".pdb");
                var pdbShouldBeLoaded = isDevelopment && fileNames.Contains(pdbPath);
                
                var dllStream = await client.GetStreamAsync(dll);
                var pdbStream = pdbShouldBeLoaded ? await client.GetStreamAsync(pdbPath) : null;

                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromStream(dllStream, pdbStream);
                    assemblies.Add(assembly);
                }
                finally
                {
                    dllStream?.Close();
                    pdbStream?.Close();
                }
            }

            var clientAssembly = Assembly.GetAssembly(typeof(App));
            assemblies.Add(clientAssembly);
            
            return assemblies;
        }
    }
}
