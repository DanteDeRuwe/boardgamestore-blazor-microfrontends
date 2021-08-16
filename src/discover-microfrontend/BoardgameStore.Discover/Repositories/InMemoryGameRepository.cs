using System.Collections.Generic;
using System.Linq;

namespace BoardgameStore.Discover.Repositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> _games = new()
        {
            new Game { Id = 1, Name = "Catan", Price = 40.00 },
            new Game { Id = 2, Name = "Carcassonne", Price = 35.00 },
            new Game { Id = 3, Name = "Pandemic", Price = 36.99 },
        };

        public Game GetBy(int id) => _games.SingleOrDefault(g => g.Id.Equals(id));
    }
}
