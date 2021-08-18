using System.Collections.Generic;
using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories
{
    public interface ICartRepository
    {
        public Cart Get();
        public void AddGame(Game game);
    }
}

