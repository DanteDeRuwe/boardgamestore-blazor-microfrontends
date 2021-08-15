using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using BoardgameStore.Client;
using BoardgameStore.Client.Routing;
using BoardgameStore.Utils;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Server
{
    internal static class Extensions
    {
        /// <summary>Registers a HttpClient that points to the server address</summary>
        internal static void AddSelfReferentialHttpClient(this IServiceCollection services)
        {
            services.AddScoped<HttpClient>(sp =>
            {
                var server = sp.GetRequiredService<IServer>();
                var baseAddress = server.Features.Get<IServerAddressesFeature>().Addresses.First();
                return new HttpClient { BaseAddress = new Uri(baseAddress) };
            });
        }

        /// <summary>Adds the microfrontend components into ComponentCollection</summary>
        internal static void AddMicrofrontends(this IServiceCollection services)
        {
            // Add the route manager for pages
            services.AddScoped<RouteManager>();
            
            // Add the components
            var dllFiles = Directory.GetFiles(@"CDN");
            var assemblies = dllFiles.Select(Assembly.LoadFrom).ToList();
            assemblies.Add(Assembly.GetAssembly(typeof(App)));
            var componentCollection = ComponentCollection.FromAssemblies(assemblies);
            services.AddScoped<ComponentCollection>(_ => componentCollection);
        }
    }
}
