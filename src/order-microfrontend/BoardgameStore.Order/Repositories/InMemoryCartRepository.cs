using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories
{
    public class InMemoryCartRepository : ICartRepository
    {
        private readonly Cart _cart = new();
        
        public Cart Get() => _cart;
        public void AddGame(Game game) => Get().Games.Add(game);
    }
}
