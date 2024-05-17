using System.Reflection;

namespace MicrofrontendFramework.Blazor;

internal class ComponentCollection : List<Type>
{
    public ComponentCollection() : base()
    {
    }

    public ComponentCollection(IEnumerable<Type> components) : base(components)
    {
    }

    public static ComponentCollection FromAssembly(Assembly assembly)
    {
        return FromTypes(assembly.GetTypes());
    }

    public static ComponentCollection FromTypes(IEnumerable<Type> types)
    {
        var components = types.Where(t => t.GetInterfaces().Contains(typeof(Microsoft.AspNetCore.Components.IComponent)));
        return new ComponentCollection(components);
    }
}