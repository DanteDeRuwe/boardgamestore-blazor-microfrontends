using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BoardgameStore.Client;
using BoardgameStore.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            var dllFiles = Directory.GetFiles(@"CDN");
            var assemblies = dllFiles.Select(Assembly.LoadFrom).ToList();
            
            var clientAssembly = Assembly.GetAssembly(typeof(App));
            assemblies.Add(clientAssembly);
            
            return assemblies;
        }
    }
}
