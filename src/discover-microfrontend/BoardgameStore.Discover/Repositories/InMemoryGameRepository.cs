namespace BoardgameStore.Discover.Repositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        public Game GetBy(int id)
        {
            return id switch
            {
                1 => new Game { Name = "Catan", Price = 40.00 },
                2 => new Game { Name = "Carcassonne", Price = 35.00 },
                3 => new Game { Name = "Pandemic", Price = 36.99 },
                _ => null
            };
        }
    }
}
