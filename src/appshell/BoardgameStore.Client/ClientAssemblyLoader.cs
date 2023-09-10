using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace BoardgameStore.Client;

internal static class ClientAssemblyLoader
{
    private const string ApiRoute = "/microfrontends";
    private const string LibraryExtension = ".dll";
    private const string LibraryExtensionPattern = @"\.dll$";
    private const string SymbolsExtension = ".pdb";

    internal static async Task<IEnumerable<Assembly>> LoadAssembliesAsync(
        HttpClient client,
        bool isDevelopment = true)
    {
        var filePaths = await client.GetFromJsonAsync<string[]>(ApiRoute);
        var dllPaths = filePaths?.Where(f => f.EndsWith(LibraryExtension)) ?? Enumerable.Empty<string>();

        var clientAssembly = Assembly.GetAssembly(typeof(App));
        var assemblies = new List<Assembly> { clientAssembly };

        foreach (var dllPath in dllPaths)
        {
            var pdbPath = Regex.Replace(dllPath, LibraryExtensionPattern, SymbolsExtension);
            var pdbShouldBeLoaded = isDevelopment && filePaths!.Contains(pdbPath);

            await using var dllStream = await client.GetStreamAsync(dllPath);
            await using var pdbStream = pdbShouldBeLoaded ? await client.GetStreamAsync(pdbPath) : null;

            var assembly = AssemblyLoadContext.Default.LoadFromStream(dllStream, pdbStream);
            assemblies.Add(assembly);
        }

        return assemblies;
    }
}