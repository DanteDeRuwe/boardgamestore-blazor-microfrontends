using System.Reflection;

namespace MicrofrontendFramework.Blazor
{
    public class AssemblyCollection : List<Assembly>
    { 
        public AssemblyCollection(IEnumerable<Assembly> assemblies) : base(assemblies) { }
    }
}
