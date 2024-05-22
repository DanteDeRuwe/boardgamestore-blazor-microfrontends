using BoardgameStore.Order.Context;
using BoardgameStore.Order.Repositories;
using MicrofrontendFramework.Blazor;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Order;

public class Microfrontend : IConfigureMicrofrontend
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CartContext>();
        services.AddSingleton<IGameRepository, InMemoryGameRepository>();
        services.AddSingleton<ICartRepository, InMemoryCartRepository>();
    }
}
