namespace BoardgameStore.Order.Context;

public class CartContext
{
    public Action CartUpdated { get; set; } = () => { };
}