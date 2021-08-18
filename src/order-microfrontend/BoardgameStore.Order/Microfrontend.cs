using BoardgameStore.Order.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BoardgameStore.Order
{
    public class Microfrontend
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IGameRepository, InMemoryGameRepository>();
            services.AddScoped<ICartRepository, InMemoryCartRepository>();
        }
    }
}
