using System.Globalization;
using BoardgameStore.Server;
using MicrofrontendFramework.Blazor;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("nl-BE");

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSelfReferentialHttpClient();

var env = builder.Environment;

var mfFileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Microfrontends"));
var assemblies = ServerAssemblyLoader.LoadAssemblies(env.IsDevelopment(), mfFileProvider);
builder.Services.AddMicrofrontends(assemblies);

var app = builder.Build();

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

// Make sure we serve the microfrontend files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = mfFileProvider,
    RequestPath = "/Microfrontends",
    ServeUnknownFileTypes = true
});

app.UseRouting();

app.MapRazorPages();

app.MapGet("/microfrontends", () => Directory.GetFiles("Microfrontends").Select(f => f.Replace('\\', '/')).Select(x => $"/{x}"));

app.MapFallbackToPage("/_Host");

await app.RunAsync();
