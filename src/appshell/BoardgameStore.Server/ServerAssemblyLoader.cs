using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using BoardgameStore.Client;

namespace BoardgameStore.Server
{
    internal static class ServerAssemblyLoader
    {
        private const string CdnFolder = "CDN";
        private const string LibraryExtension = ".dll";
        private const string LibraryExtensionPattern = @"\.dll$";
        private const string SymbolsExtension = ".pdb";

        internal static IEnumerable<Assembly> LoadAssemblies()
        {
            var dllPaths = Directory.GetFiles(CdnFolder).Where(f => f.EndsWith(LibraryExtension));

            var clientAssembly = Assembly.GetAssembly(typeof(App));
            var assemblies = new List<Assembly> { clientAssembly };

            foreach (var dllPath in dllPaths)
            {
                var pdbPath = Regex.Replace(dllPath, LibraryExtensionPattern, SymbolsExtension);

                using var pdbStream = File.Exists(pdbPath) ? File.Open(pdbPath, FileMode.Open) : null;
                using var dllStream = File.Open(dllPath, FileMode.Open);

                var assembly = AssemblyLoadContext.Default.LoadFromStream(dllStream, pdbStream);
                assemblies.Add(assembly);
            }

            return assemblies;
        }
    }
}
