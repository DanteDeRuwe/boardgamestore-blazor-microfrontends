using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories;

public class InMemoryCartRepository : ICartRepository
{
    private readonly Cart _cart = new();

    public int ItemCount => _cart.Games.Count;

    public IEnumerable<Game> GetGames()
    {
        return _cart.Games;
    }

    public void AddGame(Game game)
    {
        _cart.Games.Add(game);
    }

    public bool HasGame(Game game)
    {
        return _cart.Games.Contains(game);
    }

    public void ClearCart()
    {
        _cart.Clear();
    }
}