public class CartItem
{
    public int CartItemId { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public virtual Product Product { get; set; }

    public decimal Total => Product.Price * Quantity;
}
