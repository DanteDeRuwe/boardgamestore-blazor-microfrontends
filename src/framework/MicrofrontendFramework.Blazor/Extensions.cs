using Microsoft.AspNetCore.Components;

namespace MicrofrontendFramework.Blazor;

public static class Extensions
{
    /// <summary> Registers a list of microfrontend assemblies into a service collection, taking care of each microfrontend's DI</summary>
    /// <param name="services">The service collection to register microfrontends onto</param>
    /// <param name="assemblies">The microfrontend assemblies</param>
    public static void AddMicrofrontends(this IServiceCollection services, AssemblyCollection assemblies)
    {
        List<Type> components = [];
        foreach (var type in assemblies.SelectMany(a => a.GetTypes()).Distinct())
        {
            var interfaces = type.GetInterfaces().ToHashSet();
            if (interfaces.Count == 0) continue;

            // type is component
            if (interfaces.Contains(typeof(IComponent)))
            {
                components.Add(type);
            }

            // type is a microfrontend configurator
            if (interfaces.Contains(typeof(IConfigureMicrofrontend)))
            {
                var configureMethod = type.GetMethod(nameof(IConfigureMicrofrontend.ConfigureServices));
                configureMethod?.Invoke(null, [services]);
            }
        }

        services.AddSingleton(assemblies); // For use in routing
        services.AddSingleton(FragmentMap.FromComponents(components)); // For use in rendering fragments
    }
}