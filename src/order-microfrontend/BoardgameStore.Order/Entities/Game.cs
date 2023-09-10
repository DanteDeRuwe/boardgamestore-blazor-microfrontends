namespace BoardgameStore.Order.Entities;

public record Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
}