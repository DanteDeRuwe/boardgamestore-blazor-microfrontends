using System;
using System.Linq;
using System.Net.Http;
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
    }
}
