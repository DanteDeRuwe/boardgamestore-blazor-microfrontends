using BoardgameStore.Client;
using BoardgameStore.Utils;
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

            var assemblies = GetAssemblies();
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

        private static List<Assembly> GetAssemblies()
        {
            var files = Directory.GetFiles(@"CDN");
            var dlls = files.Where(f => f.EndsWith("dll"));

            var assemblies = new List<Assembly>();

            foreach (var dll in dlls)
            {
                var pdbPath = Regex.Replace(dll, @"\.dll$", ".pdb");
                var isPdbPresent = File.Exists(pdbPath);
                
                var pdbStream = isPdbPresent ? File.Open(pdbPath, FileMode.Open) : null;
                var dllStream = File.Open(dll, FileMode.Open);
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
