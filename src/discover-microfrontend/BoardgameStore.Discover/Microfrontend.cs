using BoardgameStore.Discover.Repositories;
using MicrofrontendFramework.Blazor;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Discover;

public class Microfrontend : IConfigureMicrofrontend
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGameRepository, InMemoryGameRepository>();
    }
}
