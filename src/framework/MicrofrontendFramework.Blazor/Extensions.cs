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

        var componentCollection = new ComponentCollection();
        foreach (var assembly in assemblyCollection)
        {
            componentCollection.AddRange(ComponentCollection.FromAssembly(assembly));
            services.ConfigureMicrofrontend(assembly);
        }

        services.AddScoped<ComponentCollection>(_ => componentCollection);
        services.AddScoped<FragmentMap>(_ => FragmentMap.FromComponents(componentCollection));
    }

    private const string MicrofrontendEntrypoint = "Microfrontend";
    private const string ConfigureServices = "ConfigureServices";

    private static void ConfigureMicrofrontend(this IServiceCollection services, Assembly assembly)
    {
        var configureServicesMethod = assembly
            .GetTypes()
            .FirstOrDefault(x => x.Name.Equals(MicrofrontendEntrypoint, StringComparison.Ordinal))
            ?.GetMethod(ConfigureServices, BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(IServiceCollection) }, null);

        configureServicesMethod?.Invoke(null, new object[] { services });
    }
}