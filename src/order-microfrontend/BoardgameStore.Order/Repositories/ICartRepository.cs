using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories
{
    public interface ICartRepository
    {
        public Cart Get();
        public void AddGame(Game game);
        public bool HasGame(Game game);
        public void ClearCart();
    }
}

