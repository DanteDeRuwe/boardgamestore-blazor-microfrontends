using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using BoardgameStore.Client;

namespace BoardgameStore.Server;

internal static class ServerAssemblyLoader
{
    private const string CdnFolder = "CDN";
    private const string LibraryExtension = ".dll";
    private const string LibraryExtensionPattern = @"\.dll$";
    private const string SymbolsExtension = ".pdb";

    internal static IEnumerable<Assembly> LoadAssemblies(bool isDevelopment)
    {
        var filePaths = Directory.GetFiles(CdnFolder);
        var dllPaths = filePaths.Where(f => f.EndsWith(LibraryExtension));

        var clientAssembly = Assembly.GetAssembly(typeof(App));
        var assemblies = new List<Assembly> { clientAssembly };

        foreach (var dllPath in dllPaths)
        {
            var pdbPath = Regex.Replace(dllPath, LibraryExtensionPattern, SymbolsExtension);
            var pdbShouldBeLoaded = isDevelopment && filePaths!.Contains(pdbPath);

            using var dllStream = File.Open(dllPath, FileMode.Open);
            using var pdbStream = pdbShouldBeLoaded ? File.Open(pdbPath, FileMode.Open) : null;

            var assembly = AssemblyLoadContext.Default.LoadFromStream(dllStream, pdbStream);
            assemblies.Add(assembly);
        }

        return assemblies;
    }
}