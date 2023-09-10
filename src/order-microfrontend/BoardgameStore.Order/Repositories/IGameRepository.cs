using BoardgameStore.Order.Entities;

namespace BoardgameStore.Order.Repositories;

public interface IGameRepository
{
    public IEnumerable<Game> GetAll();
    public Game GetBy(int id);
}