using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using BoardgameStore.Client;
using BoardgameStore.Client.Routing;
using BoardgameStore.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace BoardgameStore.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<HttpClient>(sp =>
            {
                // Get the address that the app is currently running at
                var server = sp.GetRequiredService<IServer>();
                var addressFeature = server.Features.Get<IServerAddressesFeature>();
                var baseAddress = addressFeature.Addresses.First();
                return new HttpClient { BaseAddress = new Uri(baseAddress) };
            });

            services.AddScoped<RouteManager>();

            //Add microfrontend components into componentcollection
            var dllFiles = Directory.GetFiles(@"CDN");
            var assemblies = dllFiles.Select(Assembly.LoadFrom).ToList();
            assemblies.Add(Assembly.GetAssembly(typeof(App))); //add app shell client assembly too for custom router

            var componentCollection = ComponentCollection.FromAssemblies(assemblies);
            services.AddScoped<ComponentCollection>(_ => componentCollection);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            // Make sure we serve the CDN files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "CDN")),
                RequestPath = "/CDN",
                ServeUnknownFileTypes = true
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
