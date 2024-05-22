using Microsoft.Extensions.DependencyInjection;

namespace MicrofrontendFramework.Blazor;

public interface IConfigureMicrofrontend
{
    public static abstract void ConfigureServices(IServiceCollection serviceCollection);
}
