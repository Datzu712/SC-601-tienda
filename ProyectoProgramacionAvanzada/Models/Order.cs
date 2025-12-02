public class Order
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public decimal Total { get; set; }

    public virtual ICollection<OrderDetail> Details { get; set; }
}
