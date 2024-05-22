using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MicrofrontendFramework.Blazor;

public static class Extensions
{
    /// <summary> Registers a list of microfrontend assemblies into a service collection, taking care of each microfrontend's DI</summary>
    /// <param name="services">The service collection to register microfrontends onto</param>
    /// <param name="assemblies">The microfrontend assemblies</param>
    public static void AddMicrofrontends(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var assemblyCollection = new AssemblyCollection(assemblies);
        services.AddSingleton(assemblyCollection);

        var components = new List<Type>();
        foreach (var assembly in assemblyCollection)
        {
            components.AddRange(assembly.GetTypesWithInterface(typeof(Microsoft.AspNetCore.Components.IComponent)));
            services.ConfigureMicrofrontend(assembly);
        }

        services.AddSingleton(FragmentMap.FromComponents(components));
    }

    private static void ConfigureMicrofrontend(this IServiceCollection services, Assembly assembly)
    {
        assembly.GetTypesWithInterface(typeof(IConfigureMicrofrontend))
            .FirstOrDefault()
            ?.GetMethod(nameof(IConfigureMicrofrontend.ConfigureServices))
            ?.Invoke(null, [services]);
    }

    private static IEnumerable<Type> GetTypesWithInterface(this Assembly assembly, Type interfaceType)
    {
        return assembly.GetTypes().Where(t => t.GetInterfaces().Contains(interfaceType));
    }
}
