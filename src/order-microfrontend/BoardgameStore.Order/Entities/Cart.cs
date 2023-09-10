namespace BoardgameStore.Order.Entities;

public class Cart
{
    public ICollection<Game> Games { get; init; }
    public Cart() => Games = new List<Game>();
    public void Clear() => Games.Clear();
}