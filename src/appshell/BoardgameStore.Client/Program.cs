using System.Globalization;
using BoardgameStore.Client;
using MicrofrontendFramework.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var assemblies = await ClientAssemblyLoader.LoadAssembliesAsync(client, builder.HostEnvironment.IsDevelopment());

builder.Services.AddMicrofrontends(assemblies);

var app = builder.Build();

await app.RunAsync();