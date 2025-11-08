using System.Globalization;
using BoardgameStore.Server;
using BoardgameStore.Server.Components;
using MicrofrontendFramework.Blazor;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var env = builder.Environment;

var mfFileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Microfrontends"));
var assemblies = ServerAssemblyLoader.LoadAssemblies(env.IsDevelopment(), mfFileProvider);
builder.Services.AddMicrofrontends(assemblies);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (env.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(assemblies.ToArray()); // server routing for microfrontend assemblies

// Serve the microfrontend files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = mfFileProvider,
    RequestPath = "/Microfrontends",
    ServeUnknownFileTypes = true
});

// List the microfrontend files via api route
app.MapGet("/microfrontends", () => Directory.GetFiles("Microfrontends").Select(f => f.Replace('\\', '/')).Select(x => $"/{x}"));

await app.RunAsync();