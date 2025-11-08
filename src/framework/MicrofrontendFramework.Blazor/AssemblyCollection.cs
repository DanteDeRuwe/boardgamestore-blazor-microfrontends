using System.Collections.ObjectModel;

namespace MicrofrontendFramework.Blazor;

public class AssemblyCollection(IEnumerable<Assembly> assemblies) : ReadOnlyCollection<Assembly>(assemblies.ToList());