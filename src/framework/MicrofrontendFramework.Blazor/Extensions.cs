namespace MicrofrontendFramework.Blazor;

public static class Extensions
{
    /// <summary> Registers a list of microfrontend assemblies into a service collection, taking care of each microfrontend's DI</summary>
    /// <param name="services">The service collection to register microfrontends onto</param>
    /// <param name="assemblies">The microfrontend assemblies</param>
    public static void AddMicrofrontends(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var assemblyCollection = new AssemblyCollection(assemblies);
        var types = assemblyCollection.SelectMany(a => a.GetTypes());
        
        var components = new List<Type>();
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces().ToHashSet();
            if (interfaces.Count == 0) continue;

            if (interfaces.Contains(typeof(Microsoft.AspNetCore.Components.IComponent)))
            {
                components.Add(type);
            }

            if (interfaces.Contains(typeof(IConfigureMicrofrontend)))
            {
                var configureMethod = type.GetMethod(nameof(IConfigureMicrofrontend.ConfigureServices));
                configureMethod?.Invoke(null, [services]);
            }
        }

        services.AddSingleton(assemblyCollection); // For use in routing
        services.AddSingleton(FragmentMap.FromComponents(components)); // For use in rendering fragments
    }
}