﻿using System.Globalization;
using BoardgameStore.Server;
using MicrofrontendFramework.Blazor;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("nl-BE");

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSelfReferentialHttpClient();

var env = builder.Environment;
var assemblies = ServerAssemblyLoader.LoadAssemblies(env.IsDevelopment());
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

// Make sure we serve the CDN files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "CDN")),
    RequestPath = "/CDN",
    ServeUnknownFileTypes = true
});

app.UseRouting();

app.MapRazorPages();

app.MapGet("/api/assemblies", () =>
{
    var files = Directory.GetFiles(@"CDN");
    var relativeUri = files.Select(f => $@"/{f.Replace('\\', '/')}");
    return Results.Ok(relativeUri);
});

app.MapFallbackToPage("/_Host");

await app.RunAsync();