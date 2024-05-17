using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories;

public class InMemoryGameRepository : IGameRepository
{
    private readonly List<Game> _games = new()
    {
        new Game { Id = 1, Name = "Catan", Price = 45.00, ImageUrl = "https://i.imgur.com/oxtSRMPs.png" },
        new Game { Id = 2, Name = "Carcassonne", Price = 35.00, ImageUrl = "https://i.imgur.com/Fxis0yls.png" },
        new Game { Id = 3, Name = "Pandemic", Price = 36.99, ImageUrl = "https://i.imgur.com/o1uJzsjs.png" },
        new Game { Id = 4, Name = "Ticket To Ride", Price = 32.99, ImageUrl = "https://i.imgur.com/Y0ygkMMs.png" },
        new Game { Id = 5, Name = "Monopoly", Price = 36.99, ImageUrl = "https://i.imgur.com/6Scpulws.png" },
        new Game { Id = 6, Name = "Uno", Price = 7.99, ImageUrl = "https://i.imgur.com/3QGQtsGs.png" },
        new Game { Id = 7, Name = "Splendor", Price = 34.99, ImageUrl = "https://i.imgur.com/WL7glE6s.png" },
        new Game { Id = 8, Name = "Everdell", Price = 62.99, ImageUrl = "https://i.imgur.com/LNLvNu1s.png" },
        new Game { Id = 9, Name = "Wingspan", Price = 43.99, ImageUrl = "https://i.imgur.com/IDaRMb9s.png" },
        new Game { Id = 10, Name = "Terraforming Mars", Price = 49.99, ImageUrl = "https://i.imgur.com/Bqaz3NQs.png" },
        new Game { Id = 11, Name = "Paleo", Price = 41.99, ImageUrl = "https://i.imgur.com/qPxK9yQs.png" }
    };

    public IEnumerable<Game> GetAll()
    {
        return _games;
    }

    public Game GetBy(int id)
    {
        return _games.SingleOrDefault(g => g.Id.Equals(id));
    }
}