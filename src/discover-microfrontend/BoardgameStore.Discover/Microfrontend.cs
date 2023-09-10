using BoardgameStore.Discover.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Discover;

public static class Microfrontend
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGameRepository, InMemoryGameRepository>();
    }
}