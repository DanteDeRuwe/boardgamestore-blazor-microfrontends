using BoardgameStore.Discover.Entities;

namespace BoardgameStore.Discover.Repositories;

public interface IGameRepository
{
    public IEnumerable<Game> GetAll();
    public Game GetBy(int id);
}