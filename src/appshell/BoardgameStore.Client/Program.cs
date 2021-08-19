using System;
using System.Net.Http;
using System.Threading.Tasks;
using MicrofrontendFramework.Blazor;
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

            var assemblies = await AssemblyLoader.LoadAssembliesAsync(client, builder.HostEnvironment.IsDevelopment());
            builder.Services.AddMicrofrontends(assemblies);
        }
    }
}
