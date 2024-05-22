namespace MicrofrontendFramework.Blazor;

public class AssemblyCollection(IEnumerable<Assembly> assemblies) : List<Assembly>(assemblies);