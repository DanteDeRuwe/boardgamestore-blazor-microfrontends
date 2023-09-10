using BoardgameStore.Discover.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Discover;

public class Microfrontend
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGameRepository, InMemoryGameRepository>();
    }
}