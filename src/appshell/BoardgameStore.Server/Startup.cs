using BoardgameStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using MicrofrontendFramework.Blazor;

namespace BoardgameStore.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSelfReferentialHttpClient();

            var assemblies = LoadAssemblies();
            services.AddMicrofrontends(assemblies);
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

        private static IEnumerable<Assembly> LoadAssemblies()
        {
            var dllPaths = Directory.GetFiles(@"CDN")
                .Where(f => f.EndsWith("dll"))
                .ToList();

            var clientAssembly = Assembly.GetAssembly(typeof(App));
            var assemblies = new List<Assembly> { clientAssembly };

            if (dllPaths.Count < 1) return assemblies;

            foreach (var dllPath in dllPaths)
            {
                var pdbPath = Regex.Replace(dllPath, @"\.dll$", ".pdb");

                using var pdbStream = File.Exists(pdbPath) ? File.Open(pdbPath, FileMode.Open) : null;
                using var dllStream = File.Open(dllPath, FileMode.Open);

                var assembly = AssemblyLoadContext.Default.LoadFromStream(dllStream, pdbStream);
                assemblies.Add(assembly);
            }
            
            return assemblies;
        }
    }
}
