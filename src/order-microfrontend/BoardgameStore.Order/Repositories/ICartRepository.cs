using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories;

public interface ICartRepository
{
    public int ItemCount { get; }


    public IEnumerable<Game> GetGames();
    public void AddGame(Game game);
    public bool HasGame(Game game);

    public void ClearCart();
}