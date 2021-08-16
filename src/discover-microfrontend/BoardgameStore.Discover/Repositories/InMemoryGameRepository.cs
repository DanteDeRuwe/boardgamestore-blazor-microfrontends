using System.Collections.Generic;
using System.Linq;
using BoardgameStore.Discover.Entities;

namespace BoardgameStore.Discover.Repositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> _games = new()
        {
            new Game { Id = 1, Name = "Catan", Price = 40.00, ImageUrl = "https://i.imgur.com/oxtSRMP.png"},
            new Game { Id = 2, Name = "Carcassonne", Price = 35.00, ImageUrl = "https://i.imgur.com/Fxis0yl.png"},
            new Game { Id = 3, Name = "Pandemic", Price = 36.99, ImageUrl = "https://i.imgur.com/o1uJzsj.png"},
        };

        public IEnumerable<Game> GetAll() => _games;
        public Game GetBy(int id) => _games.SingleOrDefault(g => g.Id.Equals(id));
    }
}
