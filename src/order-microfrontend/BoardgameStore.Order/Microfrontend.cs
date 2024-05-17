using BoardgameStore.Order.Context;
using BoardgameStore.Order.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Order;

public static class Microfrontend
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CartContext>();
        services.AddSingleton<IGameRepository, InMemoryGameRepository>();
        services.AddSingleton<ICartRepository, InMemoryCartRepository>();
    }
}